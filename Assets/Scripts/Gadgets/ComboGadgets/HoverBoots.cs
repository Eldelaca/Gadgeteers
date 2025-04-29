using Player.PlayerCharacterController;
using UnityEngine;

namespace Gadgets.BaseGadgets
{
    public class HoverBoots : MonoBehaviour, IGadget
    {
        public GadgetStats stats;
        public PlayerMovement playerMovement;

        private CharacterController _characterController;

        private float hoverTimer = 0f;
        private bool isHovering = false;
        private float startHoverHeight = 0f;

        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player not found");
                return;
            }

            playerMovement = player.GetComponent<PlayerMovement>();
            _characterController = player.GetComponent<CharacterController>();

            if (playerMovement == null)
                playerMovement = player.AddComponent<PlayerMovement>();
        }

        private void Update()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            HandleHover();
        }

        private void HandleHover()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (!isHovering)
                {
                    StartHover();
                }

                hoverTimer += Time.deltaTime;

                // Calculate how high player has risen
                float currentHeight = _characterController.transform.position.y - startHoverHeight;

                if (hoverTimer <= stats.useDuration && currentHeight < stats.range)
                {
                    // Move player upward slowly
                    Vector3 upwardMove = Vector3.up * stats.useCooldown * Time.deltaTime;
                    _characterController.Move(upwardMove);
                }
                else
                {
                    // Timer expired or max height reached, Start slow fall
                    playerMovement.GravityModification(stats.gravityMultiplier);
                }
            }
            else
            {
                if (isHovering)
                {
                    EndHover();
                }
            }
        }

        private void StartHover()
        {
            isHovering = true;
            hoverTimer = 0f;
            startHoverHeight = _characterController.transform.position.y;

            // Disable gravity while rising
            playerMovement.GravityModification(0f);

            Debug.Log("Started hovering");
        }

        private void EndHover()
        {
            isHovering = false;
            hoverTimer = 0f;
            startHoverHeight = 0f;

            // Restore normal gravity
            playerMovement.GravityModification(1f);

            Debug.Log("Ended hovering, falling normally");
        }

        public void Equip()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            playerMovement.JumpModification(stats.additionalJumpCount, stats.additionalJumpForce);
            GadgetManager.Instance.bootsEquipped = true;
        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            playerMovement.JumpModification(0, 1f);
            playerMovement.GravityModification(1f);

            hoverTimer = 0f;
            isHovering = false;
            GadgetManager.Instance.bootsEquipped = false;
        }

        public void UseGadget()
        {
            // No special Use needed for HoverBoots
        }
    }
}
