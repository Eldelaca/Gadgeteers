using System.Collections;
using System.Collections.Generic;
using Player.Inventory;
using UnityEngine;
using Player.PlayerCharacterController;

namespace Gadgets
{
    public class GrabberBox : MonoBehaviour
    {
        public GadgetStats stats;
        
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private PlayerLocomotionInput playerLocomotionInput;
        [SerializeField] private PlayerAnimation playerAnimation;

        private void OnTriggerEnter(Collider other)
        {
            /*playerMovement.enabled = false;
            playerLocomotionInput.enabled = false;
            playerAnimation.enabled = false;
            characterController.enabled = false; */
            
            
            if (!other.CompareTag("Player")) return;
            
            AddGadgetToInventory();
            
            Destroy(gameObject);
            // PlayNewGadgetAnimation();
        }
        
        private void AddGadgetToInventory()
        {
            InventoryManager.Instance.AddGadget(stats.gadgetId, stats.gadgetName, stats.gadgetIcon, stats.gadgetDescription);
        }

        private void PlayNewGadgetAnimation()
        {
            StartCoroutine(GadgetAnimation());
        }

        private IEnumerator GadgetAnimation()
        {
            // For later
            
            yield return new WaitForSeconds(5f);
        }
    }
}
