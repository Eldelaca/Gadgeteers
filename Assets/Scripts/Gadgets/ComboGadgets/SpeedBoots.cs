using Player.PlayerCharacterController;
using UnityEngine;

namespace Gadgets.BaseGadgets
{
    public class GravityAmplifier : MonoBehaviour, IGadget
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

            playerMovement.SpeedModification(stats.gravityMultiplier);
        }
        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != stats.range) return;

            playerMovement.SpeedModification(1f);
        }


        public void UseGadget()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            // If you want to trigger something actively here, you can.
            Debug.LogWarning("Gravity Amplifier gadget used.");
        }
    }
}
