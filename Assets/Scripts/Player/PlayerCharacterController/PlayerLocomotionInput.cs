using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.PlayerCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerInputs.IPlayerActions
    {
        #region Variables
        [SerializeField] private bool holdToSprint = true;
        private PlayerInputs PlayerInputs {get; set;}
        public Vector2 MovementInput {get; private set;} 
        public Vector2 LookInput {get; private set;}
        public bool SprintToggled {get; private set;}
        public bool JumpPressed {get; private set;}
        #endregion
        
        #region Unity Lifecycle
        private void OnEnable() // using OnEnable allows me to instantiate the player inputs when the scene first runs
        {
            PlayerInputs = new PlayerInputs();
            PlayerInputs.Enable();

            PlayerInputs.Player.Enable();
            PlayerInputs.Player.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerInputs.Player.Disable();
            PlayerInputs.Player.RemoveCallbacks(this);
        }
        #endregion
        
        #region Unity Late-Update
        private void LateUpdate()
        {
            JumpPressed = false;
        }
        #endregion
        
        #region PlayerInputs
        public void OnMove(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            // potentially needed, consult me if you're looking at this
        }

        public void OnFlash(InputAction.CallbackContext context)
        {
            // not needed for this project
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintToggled = holdToSprint || !SprintToggled;
            }
            else if (context.canceled)
            {
                SprintToggled = !holdToSprint && SprintToggled;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            JumpPressed = true;

        }
        #endregion
    }

}
