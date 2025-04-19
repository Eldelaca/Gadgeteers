using Gadgets;
using UI.Inventory;
using UnityEngine;

namespace Player.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        private int _pushedGadgetID;
        private string _pushedGadgetName;
        private Sprite _pushedGadgetSprite;
        
        public ItemSlot[] inventorySlots;

        public int collectibleCount;
        
        public static InventoryManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one instance of InventoryManager");
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void AddGadget(int gadgetID, string gadgetName, Sprite gadgetSprite, string gadgetDescription)
        {
            _pushedGadgetID = gadgetID;
            _pushedGadgetName = gadgetName;
            _pushedGadgetSprite = gadgetSprite;
            
            switch (GameObject.FindGameObjectsWithTag("Gadget").Length != 0)
            {
                case true when _pushedGadgetID != GadgetManager.Instance.equippedID:
                    GadgetManager.Instance.OnUnEquip();
                    break;
                case true when _pushedGadgetID == GadgetManager.Instance.equippedID:
                    Debug.Log("That gadget is already equipped");
                    return;
                case false:
                    break;
                default:
                    Debug.LogError("Something has gone very wrong in the switch statement");
                    break;
            }
            
            GadgetManager.Instance.OnEquip(_pushedGadgetID);

            foreach (ItemSlot inventorySlot in inventorySlots)
            {
                if (!inventorySlot.slotFull)
                {
                    inventorySlot.FillSlot(_pushedGadgetID, _pushedGadgetName, _pushedGadgetSprite);
                    return;
                }
            }


        }

        public void AddCollectible()
        {
            collectibleCount++;
        }
        
        // private void EquipGadget()
    }
}
