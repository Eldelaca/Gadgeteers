using Gadgets;
using UI.Inventory;
using UnityEngine;

namespace Player.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        private int _pushedGadgetID;
        private string _pushedGadgetName, _pushedGadgetDescription;
        private Sprite _pushedGadgetSprite;
        private GadgetStats _inputGadget1, _inputGadget2;

        public CollectibleUI collectibleUI;


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

        public void AddGadget(int gadgetID, string gadgetName, Sprite gadgetSprite, string gadgetDescription, bool isCombo, GadgetStats inputGadget1, GadgetStats inputGadget2)
        {
            _pushedGadgetID = gadgetID;
            _pushedGadgetName = gadgetName;
            _pushedGadgetSprite = gadgetSprite;
            _pushedGadgetDescription = gadgetDescription;

            foreach (ItemSlot inventorySlot in inventorySlots)
            {
                if (inventorySlot.slotFull) continue;
                inventorySlot.FillSlot(_pushedGadgetID, _pushedGadgetName, _pushedGadgetSprite, _pushedGadgetDescription, isCombo, inputGadget1, inputGadget2);
                return;
            }
        }
        
        public void AddCollectible()
        {
            collectibleCount++;
            collectibleUI.ChangeCollectibleCount(collectibleCount);
        }
    }
}
