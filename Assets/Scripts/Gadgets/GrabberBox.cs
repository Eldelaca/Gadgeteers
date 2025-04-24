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
            if (!other.CompareTag("Player")) return;
            
            AddGadgetToInventory();
            
            Destroy(gameObject);
        }
        
        private void AddGadgetToInventory()
        {
            InventoryManager.Instance.AddGadget(stats.gadgetId, stats.gadgetName, stats.gadgetIcon, stats.gadgetDescription, false, null, null);
        }
    }
}
