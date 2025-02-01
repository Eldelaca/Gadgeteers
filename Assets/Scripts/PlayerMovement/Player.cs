using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerCamera playerCamera;
    [SerializeField]
    private Transform cameraFollowPoint;
    [SerializeField]
    private CharacterController characterController;

    private Vector3 lookInputVector;

    private void Start()
    {
        playerCamera.SetFollowTransform(cameraFollowPoint);
    }

    private void HandleCameraInput()
    {
        float mouseUp = Input.GetAxis("Mouse Y");
        float mouseRight = Input.GetAxis("Mouse X");
        
        lookInputVector = new Vector3(mouseRight, mouseUp, 0f);
        
        float scrollInput = -Input.GetAxis("Mouse ScrollWheel");
        playerCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);
    }

    private void HandleCharacterInputs()
    {
        PlayerInputs inputs = new PlayerInputs();
        inputs.MoveAxisForward = Input.GetAxis("Vertical");
        inputs.MoveAxisRight = Input.GetAxis("Horizontal");
        inputs.CameraRotation = playerCamera.transform.rotation;
        inputs.JumpPressed = Input.GetButtonDown("Jump");
        
        characterController.SetInputs(ref inputs);
    }

    private void Update()
    {
        HandleCharacterInputs();
    }

    private void LateUpdate()
    {
        HandleCameraInput();
    }
}
