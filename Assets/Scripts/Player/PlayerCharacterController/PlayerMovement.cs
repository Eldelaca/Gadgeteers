using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.PlayerCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerMovement : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private LayerMask groundLayers;

        public float RotationMismatch { get; private set; } = 0f;
        public bool IsRotatingToTarget { get; private set; } = false;
        
        [Header("Lateral Movement Settings")]
        [SerializeField] private float walkAcceleration = 0.15f;
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runAcceleration = 0.25f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float sprintAcceleration = 0.5f;
        [SerializeField] private float sprintSpeed = 7f;
        [SerializeField] private float drag = 0.1f;
        [SerializeField] private float movingThreshold = 0.01f;

        
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 1f;
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private float inAirAcceleration = 0.2f;

        [Header("Animation Settings")] 
        [SerializeField] private float playerModelRotationSpeed = 10f;
        [SerializeField] private float rotateToTargetTime= 0.25f;
        
        [Header("Camera Settings")]
        [SerializeField] private float lookSenseH = 0.1f;
        [SerializeField] private float lookSenseV = 0.1f;
        [SerializeField] private float lookLimitV = 89f;
        
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerStateMachine _playerStateMachine;
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;

        private bool _isRotatingClockWise;
        private float _rotatingToTargetTimer;
        private float _playerVerticalVelocity;
        private int _jumpCounter;
        private bool _jumpedLastFrame;
        private float _antiBump;
        private float _stepOffset;
        private float _modifiedGravity;
        private float _modifiedSprintSpeed;


        private int _additionalJumps;
        private float _additionalJumpForce = 1f;
        private float _additionalSpeed;
        private float _modifiedSprintAcceleration = 1f;


        #endregion

        #region Startup
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerStateMachine = GetComponent<PlayerStateMachine>();

            _antiBump = sprintSpeed;
            _stepOffset = characterController.stepOffset;
            _modifiedGravity = gravity;
            _modifiedSprintSpeed = sprintSpeed;
        }
        #endregion

        #region Update Logic    

        private void Update()
        {
            UpdateMovementState();
            HandleVerticalMovement();
            HandleLateralMovement();
        }

        private void UpdateMovementState()
        {
            bool canRun = CanRun();
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
            bool isMovingLaterally = IsMovingLaterally();
            bool isSprinting = _playerLocomotionInput.SprintToggled && IsMovingLaterally();
            bool isWalking = (isMovingLaterally && !canRun);
            bool isGrounded = IsGrounded();

            PlayerMovementState lateralState = isWalking ? PlayerMovementState.Walking :
                                                isSprinting ? PlayerMovementState.Sprinting :
                                                isMovingLaterally || isMovementInput ? PlayerMovementState.Running :
                                                PlayerMovementState.Idling;
            
            _playerStateMachine.SetPlayerMovementState(lateralState);

            if ((!isGrounded || _jumpedLastFrame) && characterController.velocity.y > 0f)
            {
                _playerStateMachine.SetPlayerMovementState(PlayerMovementState.Jumping);
                _jumpedLastFrame = false;
                characterController.stepOffset = 0f;
            }
            else if ((!isGrounded || _jumpedLastFrame) && characterController.velocity.y <= 0f)
            {
                _playerStateMachine.SetPlayerMovementState(PlayerMovementState.Falling);
                _jumpedLastFrame = false;
                characterController.stepOffset = 0f;
            }
            else
            {
                characterController.stepOffset = _stepOffset;
                _jumpCounter = 1 + _additionalJumps;
            }
        }

        private void HandleVerticalMovement()
        {
            bool isGrounded = IsGrounded();
            
            _playerVerticalVelocity -= _modifiedGravity * Time.deltaTime;
            
            if (isGrounded && _playerVerticalVelocity < 0f) _playerVerticalVelocity = -_antiBump;
            
            float modifiedJumpForce = jumpForce * _additionalJumpForce;

            if ((!_playerLocomotionInput.JumpPressed || !isGrounded)&&
                (!_playerLocomotionInput.JumpPressed || isGrounded || _jumpCounter < 1)) return;
            _jumpCounter--;
            _playerVerticalVelocity = 0f;
            _playerVerticalVelocity += _antiBump + Mathf.Sqrt(modifiedJumpForce* 3.0f * _modifiedGravity);
            _jumpedLastFrame = true;
        }

        private void HandleLateralMovement()
        {
            bool isSprinting = _playerStateMachine.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isGrounded = _playerStateMachine.InGroundedState();
            bool isWalking = _playerStateMachine.CurrentPlayerMovementState == PlayerMovementState.Walking;

            float lateralAcceleration = !isGrounded ? inAirAcceleration :
                isWalking ? walkAcceleration :
                isSprinting ? _modifiedSprintAcceleration : runAcceleration;
            float clampLateralMagnitude = isGrounded ? _modifiedSprintSpeed :
                isWalking ? walkSpeed :
                isSprinting ? _modifiedSprintSpeed : runSpeed;

            Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0f, playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

            Vector3 movementDelta = movementDirection * lateralAcceleration;
            Vector3 newVelocity = characterController.velocity + movementDelta;

            Vector3 currentDrag = newVelocity.normalized * drag;
            newVelocity = (newVelocity.magnitude > drag) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x, 0f, newVelocity.z), clampLateralMagnitude);
            newVelocity.y += _playerVerticalVelocity;

            characterController.Move(newVelocity * Time.deltaTime);
        }

        #endregion

        #region Late-Update Logic

        private void LateUpdate()
        {
            HandleCameraMovement();
        }

        private void HandleCameraMovement()
        {
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);
            
            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;
            const float rotationTolerance = 90f;
            bool isIdling = _playerStateMachine.CurrentPlayerMovementState == PlayerMovementState.Idling;
            IsRotatingToTarget = _rotatingToTargetTimer > 0;

            if (!isIdling) RotatePlayerToTarget();
            
            else if (Mathf.Abs(RotationMismatch) > rotationTolerance || IsRotatingToTarget) UpdateIdleRotation(rotationTolerance);
            
            playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y,_cameraRotation.x, 0f);
            
            Vector3 camForwardProjectedXZ = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
            Vector3 crossProduct =  Vector3.Cross(transform.forward, camForwardProjectedXZ);
            float sign =  Mathf.Sign(Vector3.Dot(crossProduct, transform.up));
            RotationMismatch = sign * Vector3.Angle(transform.forward, camForwardProjectedXZ);
            
        }
        
        private void UpdateIdleRotation(float rotationTolerance)
        {
            if (Mathf.Abs(RotationMismatch) > rotationTolerance)
            {
                _rotatingToTargetTimer = rotateToTargetTime;
                _isRotatingClockWise = RotationMismatch > rotationTolerance;
            }
            _rotatingToTargetTimer -= Time.deltaTime;

            if (_isRotatingClockWise && RotationMismatch > 0f ||
                !_isRotatingClockWise && RotationMismatch < 0f)
                RotatePlayerToTarget();

        }
        
        private void RotatePlayerToTarget()
        {
            Quaternion targetRotationX = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationX, playerModelRotationSpeed * Time.deltaTime);
        }
        
        #endregion   
        
        #region State Checks
        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);

            return lateralVelocity.magnitude > movingThreshold;
        }

        private bool IsGrounded()   
        {
            bool grounded = _playerStateMachine.InGroundedState() ? IsGroundedWhileGrounded() : IsGroundedWhileAirborne();
            
            return grounded;
        }

        private bool IsGroundedWhileGrounded()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - characterController.radius, transform.position.z);

            bool grounded = Physics.CheckSphere(spherePosition, characterController.radius, groundLayers, QueryTriggerInteraction.Ignore);

            return grounded;
        }

        private bool IsGroundedWhileAirborne()
        {
            return characterController.isGrounded;
        }

        private bool CanRun()
        {
            return _playerLocomotionInput.MovementInput.y >= Math.Abs(_playerLocomotionInput.MovementInput.x);
        }
        #endregion
        
        #region Gadget Checks
        public void JumpModification(int additionalJumps, float additionalJumpForce)
        {
            _additionalJumps = additionalJumps; 
            _additionalJumpForce = additionalJumpForce;
        }

        public void GravityModification(float gravityModifier)
        {
            _modifiedGravity = gravity * gravityModifier;
        }

        public void SpeedModification(float speedModifier)
        {
            _modifiedSprintSpeed = sprintSpeed * speedModifier;
            _modifiedSprintAcceleration = sprintAcceleration * speedModifier;
        }

        #endregion


    }
}

