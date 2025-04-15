using UnityEngine;

namespace Player.PlayerCharacterController
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [field: SerializeField] public PlayerMovementState CurrentPlayerMovementState { get; private set; } = PlayerMovementState.Idling;

        public void SetPlayerMovementState(PlayerMovementState playerMovementState)
        {
            CurrentPlayerMovementState = playerMovementState;
        }

        public bool InGroundedState()
        {
            return CurrentPlayerMovementState == PlayerMovementState.Idling || 
                   CurrentPlayerMovementState == PlayerMovementState.Walking || 
                   CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        }
    }
    
    public enum PlayerMovementState
    {
        Idling = 0,
        Walking = 1,
        Sprinting = 2,
        Strafing = 3,
        Falling = 4,
        Jumping = 5,
    }
}
