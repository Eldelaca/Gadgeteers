using Player.PlayerCharacterController;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gadgets.BaseGadgets
{
    public class RocketBoots : MonoBehaviour, IGadget
    {
        public GadgetStats stats;
        public PlayerMovement playerMovement;
        
        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerMovement = player.GetComponent<PlayerMovement>();

            if (player == null)
            {
                Debug.LogError("Player not found");
            }
            
            if (playerMovement == null)
            {
                playerMovement = player.AddComponent<PlayerMovement>();
            }
        }
        
        public void Equip()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            playerMovement.JumpModification(stats.additionalJumpCount, stats.additionalJumpForce);
            GadgetManager.Instance.bootsEquipped = true; // only for debug, remove on final iteration
        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;
            
            playerMovement.JumpModification(0, 1f);
            GadgetManager.Instance.bootsEquipped = false; // only for debug, remove on final iteration
        }

        public void UseGadget()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;
            
            // Debug to make sure that nothing goes wrong :3
            Debug.LogError("For some reason RocketBoots are being used, fix it right neow");
        }
    }
}
