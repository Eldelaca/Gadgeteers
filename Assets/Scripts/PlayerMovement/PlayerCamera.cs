using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] 
    private float defaultDistance = 6f,
        minDistance = 3f,
        maxDistance = 10f,
        distanceMovementSpeed = 5f,
        distanceMovementSharpness = 10f,
        rotationSpeed = 10f,
        rotationSharpness = 10000f,
        followSharpness = 10000f,
        minVerticalAngle = -90f,
        maxVerticalAngle = 20f,
        defaultVerticalAngle = 20f;

    [Header("References")] 
    private Transform followTransform;
    private Vector3 currentFollowPosition, planarDirection;
    private float targetVerticalAngle;
    
    private float currentDistance, targetDistance;

    private void Awake()
    {
        currentDistance = defaultDistance;
        targetDistance = currentDistance;
        targetVerticalAngle = 0f;
        planarDirection = Vector3.forward;
    }

    public void SetFollowTransform(Transform t)
    {
        followTransform = t;
        currentFollowPosition = t.position;
        planarDirection = t.forward;
    }

    private void OnValidate()
    {
        defaultDistance = Mathf.Clamp(defaultDistance, minDistance, maxDistance);
        defaultVerticalAngle = Mathf.Clamp(defaultVerticalAngle, minVerticalAngle, maxVerticalAngle);
    }

    private void HandleRotationInput(float deltaTime, Vector3 rotationInput, out Quaternion targetRotation)
    {
        Quaternion rotationFromInput = Quaternion.Euler(followTransform.up * (rotationInput.x * rotationSpeed));
        planarDirection = rotationFromInput * planarDirection;
        Quaternion planarRot = Quaternion.LookRotation(planarDirection, followTransform.up);
        
        targetVerticalAngle -= (rotationInput.y * rotationSpeed);
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, minVerticalAngle, maxVerticalAngle);
        Quaternion verticalRot = Quaternion.Euler(targetVerticalAngle, 0, 0);
        
        targetRotation = Quaternion.Slerp(transform.rotation, planarRot * verticalRot, rotationSharpness * deltaTime);
        
        transform.rotation = targetRotation;
    }

    private void HandlePosition(float deltaTime, float zoomInput, Quaternion targetRotation)
    {
        targetDistance += zoomInput * distanceMovementSpeed;
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        
        currentFollowPosition = Vector3.Lerp(currentFollowPosition, followTransform.position, 1f - Mathf.Exp(-followSharpness * deltaTime));
        Vector3 targetPostion = currentFollowPosition - ((targetRotation * Vector3.forward) * currentDistance);
        
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, 1f - Mathf.Exp(-distanceMovementSharpness * deltaTime));
        transform.position = targetPostion;
    }

    public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
    {
        if (followTransform)
        {
            HandleRotationInput(deltaTime, rotationInput, out Quaternion targetRotation);
            HandlePosition(deltaTime, zoomInput, targetRotation);
        }
    }
}
