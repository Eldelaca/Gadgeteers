using Player.PlayerCharacterController;
using UnityEngine;

namespace Gadgets.BaseGadgets
{
    public class ExplosiveBoots : MonoBehaviour, IGadget
    {
        public GadgetStats stats;
        public PlayerMovement playerMovement;
        private PlayerLocomotionInput locInput;

        private BoxCollider explosionCollider;
        private bool readyToExplode = false;
        private int jumpCount = 0;
        private bool wasGroundedLastFrame = true;

        private void Awake()
        {
            // grab the trigger-collider we added
            explosionCollider = GetComponent<BoxCollider>();
            if (explosionCollider == null)
            {
                Debug.LogError("ExplosiveBoots needs a BoxCollider!");
            }
            else
            {
                explosionCollider.enabled = false;
            }

            var player = GameObject.FindGameObjectWithTag("Player");
            playerMovement = player.GetComponent<PlayerMovement>();

            // grab the input so we can count the Jumps being pressed
            locInput = player.GetComponent<PlayerLocomotionInput>();
            if (locInput == null)
            {
                Debug.LogError("ExplosiveBoots needs PlayerLocomotionInput!");
            }
        }

        private void Update()
        {
            if (!GadgetManager.Instance.bootsEquipped || playerMovement == null)
                return;

            bool isGrounded = playerMovement.GetComponent<CharacterController>().isGrounded;

            // Count each time a Jump is pressed
            if (locInput.JumpPressed)
            {
                jumpCount++;
                int required = 1 + stats.additionalJumpCount;
                if (jumpCount >= required)
                {
                    readyToExplode = true;
                }
            }

            // When we jump more than once we trigger the explosion
            if (isGrounded && !wasGroundedLastFrame && readyToExplode)
            {
                StartCoroutine(DoExplosion());
                readyToExplode = false;
                jumpCount = 0;
            }

            // resets when we are on the ground
            if (isGrounded)
            {
                jumpCount = 0;
                readyToExplode = false;
            }

            wasGroundedLastFrame = isGrounded;
        }

        private System.Collections.IEnumerator DoExplosion()
        {
           
            explosionCollider.enabled = true;

            yield return new WaitForFixedUpdate();

            explosionCollider.enabled = false;
        }

        // Damaging Code
        private void OnTriggerEnter(Collider other)
        {
            // damage only once per object
            var health = other.GetComponent<aiHealth>();
            if (health != null)
                health.TakeDamage(stats.gadgetMaxDamage);
        }


        // Equipping Methods
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
            GadgetManager.Instance.bootsEquipped = false;
        }

        public void UseGadget() 
        { 
            // Not being used atm...
        
        }

        
    }
}