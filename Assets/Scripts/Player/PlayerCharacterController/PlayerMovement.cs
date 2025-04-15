using System;
using UnityEngine;

namespace Player.PlayerCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerMovement : MonoBehaviour
    {
        #region Variables
        [Header("References")] // References for CharacterController and CameraPivot
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera playerCamera;

        public float RotationMismatch { get; private set; } = 0f;
        public bool IsRotatingToTarget { get; private set; } = false;

        [Header("Lateral Movement Settings")] // Player Movement values can be edited in the editor
        [SerializeField] private float movementAcceleration = 0.5f;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float sprintAcceleration = 0.5f;
        [SerializeField] private float sprintSpeed = 5f;
        [SerializeField] private float drag = 0.1f;
        [SerializeField] private float movingThreshold = 0.01f;
        
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 1f;
        [SerializeField] private float gravity = -9.81f;

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
        
        private bool _isRotatingClockWise = false;
        private float _rotatingToTargetTimer;
        private float _playerVerticalVelocity = 0f;

        #endregion
        
        #region Startup
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>(); 
            _playerStateMachine = GetComponent<PlayerStateMachine>();
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
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
            bool isMovingLaterally = IsMovingLaterally();
            bool isSprinting = _playerLocomotionInput.SprintToggled && IsMovingLaterally();
            bool isGrounded = IsGrounded();

            PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                isMovingLaterally || isMovementInput ? PlayerMovementState.Walking : PlayerMovementState.Idling;
            
            _playerStateMachine.SetPlayerMovementState(lateralState);

            if (!isGrounded && characterController.velocity.y >= 0f)
            {
                _playerStateMachine.SetPlayerMovementState(PlayerMovementState.Jumping);
            }
            
            else if (!isGrounded && characterController.velocity.y < 0f)
            {
                _playerStateMachine.SetPlayerMovementState(PlayerMovementState.Falling);
            }
            
        }

        private void HandleVerticalMovement()
        {
            bool isGrounded = _playerStateMachine.InGroundedState();

            if (isGrounded && _playerVerticalVelocity < 0f) 
                _playerVerticalVelocity = 0f;
            
            _playerVerticalVelocity -= gravity * Time.deltaTime;

            if (_playerLocomotionInput.JumpPressed && isGrounded)
            {
                _playerVerticalVelocity += Mathf.Sqrt(jumpForce * 3.0f * gravity);
            }

        }

        private void HandleLateralMovement()
        {
            bool isSprinting = _playerStateMachine.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isGrounded = _playerStateMachine.InGroundedState();
            
            float lateralAcceleration = isSprinting ? sprintAcceleration : movementAcceleration;
            float clampLateralMagnitude = isSprinting ? sprintSpeed : movementSpeed;
            
            Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0f, playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;
            
            Vector3 movementDelta = movementDirection * (lateralAcceleration * Time.deltaTime);
            Vector3 newVelocity = characterController.velocity + movementDelta;
            
            Vector3 currentDrag = newVelocity.normalized * (drag * Time.deltaTime);
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(newVelocity, clampLateralMagnitude);
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

            if (!isIdling)
            {
                RotatePlayerToTarget();
            }
            
            else if (Mathf.Abs(RotationMismatch) > rotationTolerance || IsRotatingToTarget)
            {
                UpdateIdleRotation(rotationTolerance);
            }
            
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
            return characterController.isGrounded;
        }
        #endregion
    }
}

