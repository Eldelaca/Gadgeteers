using KinematicCharacterController;
using UnityEngine;

public struct PlayerInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public Quaternion CameraRotation;
    public bool JumpPressed;
}

public class CharacterController : MonoBehaviour, ICharacterController
{
    [SerializeField] 
    private KinematicCharacterMotor motor;
    
    [SerializeField]
    private Vector3 gravity = new Vector3(0, -9.81f, 0);

    [SerializeField]
    private float maxStableMoveSpeed = 10f, 
        stableMovementSharpness = 15f, 
        orientationSharpness = 10f,
        _jumpSpeed = 10f;
    
    private Vector3 _moveInputVector, _lookInputVector;
    private bool _jumpRequested;
    private bool _jumped = false;
    
    

    private void Start()
    {
        motor.CharacterController = this;
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
        }

        if (_jumpRequested && motor.GroundingStatus.IsStableOnGround)
        {
            currentVelocity += (motor.CharacterUp * _jumpSpeed) - Vector3.Project(currentVelocity, motor.CharacterUp);
            _jumpRequested = false;
            motor.ForceUnground();
            _jumped = true;
        }
        else if (_jumpRequested && _jumped)
        {
             currentVelocity += (motor.CharacterUp * _jumpSpeed) - Vector3.Project(currentVelocity, motor.CharacterUp);
             _jumpRequested = false;
             _jumped = false;
        }
        
    }
}
