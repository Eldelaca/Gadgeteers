using UnityEngine;

namespace Player.PlayerCharacterController
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float locomotionBlendSpeed = 0.02f;
        
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerStateMachine _playerStateMachine;
        private PlayerMovement _playerMovement;
        
        // Avoids calling the same string every time, uses a single reference instead
        private static readonly int InputXHash = Animator.StringToHash("inputX"); 
        private static readonly int InputYHash = Animator.StringToHash("inputY");
        private static readonly int InputMagnitudeHash = Animator.StringToHash("inputMagnitude");
        private static readonly int RotationMismatchHash = Animator.StringToHash("rotationMismatch");
        private static readonly int IsIdlingHash = Animator.StringToHash("isIdling");
        private static readonly int IsJumpingHash = Animator.StringToHash("isJumping");
        private static readonly int IsGroundedHash = Animator.StringToHash("isGrounded");
        private static readonly int IsFallingHash = Animator.StringToHash("isFalling");
        private static readonly int IsRotatingToTargetHash = Animator.StringToHash("isRotatingToTarget");
        
        
        private Vector3 _currentBlendInput = Vector3.zero;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>(); // Reference to Player Input script
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            bool isSprinting = _playerStateMachine.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isJumping = _playerStateMachine.CurrentPlayerMovementState == PlayerMovementState.Jumping;
            bool isIdling = _playerStateMachine.CurrentPlayerMovementState == PlayerMovementState.Idling;
            bool isFalling = _playerStateMachine.CurrentPlayerMovementState == PlayerMovementState.Falling;
            bool isGrounded = _playerStateMachine.InGroundedState();
            
            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * 1.5f : _playerLocomotionInput.MovementInput;
            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed *Time.deltaTime);
                        
            animator.SetBool(IsRotatingToTargetHash, _playerMovement.IsRotatingToTarget);
            animator.SetBool(IsJumpingHash, isJumping);
            animator.SetBool(IsGroundedHash, isGrounded);
            animator.SetBool(IsFallingHash, isFalling);
            animator.SetBool(IsIdlingHash, isIdling);
            animator.SetFloat(InputXHash, _currentBlendInput.x);
            animator.SetFloat(InputYHash, _currentBlendInput.y);
            animator.SetFloat(InputMagnitudeHash, _currentBlendInput.magnitude);
            animator.SetFloat(RotationMismatchHash, _playerMovement.RotationMismatch);
        }
    }
}
