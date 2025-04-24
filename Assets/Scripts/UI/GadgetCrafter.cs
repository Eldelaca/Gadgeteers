using System.Collections.Generic;
using System.Linq;
using Gadgets;
using Player.Inventory;
using UI.Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class GadgetCrafter : MonoBehaviour
    {
        [SerializeField] private CombineSlot[] combineSlots;
        [SerializeField] private ComboGadgetStats[] comboGadgets;
        [SerializeField] private GameObject combinedGadgetSlot;
        [SerializeField] private Image combinedGadgetImage;
        [SerializeField] private Button combinedGadgetButton;
        
        private GadgetStats[] _gadgetInputs;
        
        private string _combinedGadgetName, _combinedGadgetDescription;
        private int _combinedGadgetID;
        private Sprite _combinedGadgetSprite;
        
        private GadgetStats _inputGadget1, _inputGadget2, _inputGadgetClass1, _inputGadgetClass2;

        private readonly int[] _passedIDs = {0, 0};

        public void OnCombineButtonClicked()
        {
            for (int i = 0; i < _passedIDs.Length; ++i)
            {
                _passedIDs[i] = combineSlots[i].storedID;
            }
            
            Debug.Log(_passedIDs[0] + " : " + _passedIDs[1]);
            
            if (_passedIDs.Any(i => i == 0))
            {
                foreach (CombineSlot combineSlot in combineSlots)
                {
                    combineSlot.ClearSlot();
                }
                return;
            }
            
            foreach (CombineSlot combineSlot in combineSlots)
            {
                combineSlot.EmptySlot();
            }
            
            CraftingManager.Instance.CraftGadget(_passedIDs[0], _passedIDs[1]);
        }

        public void FillComboGadgetSlot(int gadgetID, string gadgetName, string gadgetDescription, Sprite gadgetSprite, GadgetStats comboComponent1, GadgetStats comboComponent2)
        {
            _combinedGadgetID = gadgetID;
            _combinedGadgetName = gadgetName;
            _combinedGadgetDescription = gadgetDescription;
            _combinedGadgetSprite = gadgetSprite;
            
            _inputGadget1 = comboComponent1;
            _inputGadget2 = comboComponent2;
            
            _gadgetInputs = new GadgetStats[combineSlots.Length];
            _gadgetInputs[0] = comboComponent1;
            _gadgetInputs[1] = comboComponent2;

            combinedGadgetImage.sprite = gadgetSprite;

            combinedGadgetButton.interactable = true;
        }

        public void AddComboToInventory()
        {
            InventoryManager.Instance.AddGadget(_combinedGadgetID, _combinedGadgetName, _combinedGadgetSprite, _combinedGadgetDescription, true, _gadgetInputs[0], _gadgetInputs[1]);
            ClearSlot();
        }

        public void ClearComboGadgetSlot()
        {
            InventoryManager.Instance.AddGadget(_inputGadget1.gadgetId, _inputGadget1.gadgetName, _inputGadget1.gadgetIcon, _inputGadget1.gadgetDescription, false, null, null);
            InventoryManager.Instance.AddGadget(_inputGadget2.gadgetId, _inputGadget2.gadgetName, _inputGadget2.gadgetIcon, _inputGadget2.gadgetDescription, false, null, null);
            
            _combinedGadgetID = 0;
            _combinedGadgetName = "";
            _combinedGadgetDescription = "";
            _combinedGadgetSprite = null;
            
            combinedGadgetImage.sprite = null;
            
            _gadgetInputs[0] = null;
            _gadgetInputs[1] = null;
            
            combinedGadgetButton.interactable = false;
        }

        private void ClearSlot()
        {
            _combinedGadgetID = 0;
            _combinedGadgetName = "";
            _combinedGadgetDescription = "";
            _combinedGadgetSprite = null;
            
            combinedGadgetImage.sprite = null;
            
            _gadgetInputs[0] = null;
            _gadgetInputs[1] = null;
            
            combinedGadgetButton.interactable = false;
        }
    }
}
