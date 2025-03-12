using System;
using KinematicCharacterController;
using UnityEngine;




public struct PlayerInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public Quaternion CameraRotation;
    public bool JumpPressed;
    public bool JumpReleased;
}



public class CharacterController : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor motor;

    [SerializeField]
    private Vector3 gravity = new Vector3(0, -9.81f, 0);

    public float maxStableMoveSpeed = 10f,
        stableMovementSharpness = 15f,
        orientationSharpness = 10f,
        coyoteTime = 0.5f,
        jumpSpeed = 10f;

    public Vector3 _moveInputVector, _lookInputVector;
    public float coyoteTimeCounter;
    public bool _jumpRequested;
    public bool _jumped = false;

    // Reference Equipment script
    public Equipment equip;

    // For Ice Boots
    [SerializeField]
    private bool isOnWater = false;

    private void Start()
    {
        motor.CharacterController = this;
    }

    // This will update whenever the player activates or deactivates a certain equipment (boots/ double jump are in movement function)
    public void Update()
    {
        if (equip._hasIceGun)
        {
            equip.IceGun();
        }
        if (equip._hasIceGun == false)
        {
            equip.IceGun();
        }

    }

    public void SetInputs(ref PlayerInputs inputs)
    {
        Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);
        Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, motor.CharacterUp).normalized;

        if (cameraPlanarDirection.sqrMagnitude == 0f)
        {
            cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, motor.CharacterUp).normalized;
        }

        Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, motor.CharacterUp);

        _moveInputVector = cameraPlanarRotation * moveInputVector;
        _lookInputVector = _moveInputVector.normalized;

        if (inputs.JumpPressed)
        {
            _jumpRequested = true;
        }
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public void PostGroundingUpdate(float deltaTime)
    {

    }

    public void AfterCharacterUpdate(float deltaTime)
    {

    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport)
    {

    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
        Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {

    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (_lookInputVector.sqrMagnitude > 0f && orientationSharpness > 0f)
        {
            if (_lookInputVector.sqrMagnitude > 0f && orientationSharpness > 0f)
            {
                Vector3 smoothLookInputDirection = Vector3.Slerp(motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-orientationSharpness * deltaTime)).normalized;

                currentRotation = Quaternion.LookRotation(smoothLookInputDirection, motor.CharacterUp);
            }
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (motor.GroundingStatus.IsStableOnGround)
        {
            coyoteTimeCounter = coyoteTime;
            float currentVelocityMagnitude = currentVelocity.magnitude;
            Vector3 effectiveGroundNormal = motor.GroundingStatus.GroundNormal;

            currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

            Vector3 inputRight = Vector3.Cross(_moveInputVector, motor.CharacterUp);
            Vector3 reorientatedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;

            Vector3 targetMovementVelocity = reorientatedInput * maxStableMoveSpeed;

            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-stableMovementSharpness * deltaTime));
        }
        else
        {
            currentVelocity += gravity * deltaTime;
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (_jumpRequested && coyoteTimeCounter > 0f)
        {
            currentVelocity += (motor.CharacterUp * jumpSpeed) - Vector3.Project(currentVelocity, motor.CharacterUp);
            motor.ForceUnground();
            _jumpRequested = false;
            _jumped = true;
            coyoteTimeCounter = 0f;


        }

        // Boots (Double Jump)
        else if (_jumpRequested && _jumped && equip._hasBoots)
        {

            currentVelocity += (motor.CharacterUp * jumpSpeed) - Vector3.Project(currentVelocity, motor.CharacterUp);
            _jumpRequested = false;
            _jumped = false;
        }


    }

   
}
