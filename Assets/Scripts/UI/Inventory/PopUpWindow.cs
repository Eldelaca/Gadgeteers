using System.Collections.Generic;
using System.Linq;
using Gadgets;
using Player.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace UI.Inventory
{
    public class PopUpWindow : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup gadgetManagerCanvasGroup;
        [SerializeField] private CombineSlot[] combineSlots;
        [SerializeField] private EquipSlot equipSlot;
        [SerializeField] private TextMeshProUGUI comboButtonText;
        
        private int _selectedGadgetID;
        private Sprite _selectedGadgetSprite;
        private List<IGadgetSlot> _gadgetSlots;
        private string _selectedGadgetName, _selectedGadgetDescription;
        private bool _isCombo;

        private GadgetStats _inputGadget1, _inputGadget2;
        
        public void OnPopUpOpen(int passedGadgetID, Sprite passedGadgetSprite, string passedGadgetName, string passedGadgetDescription, bool isCombo, GadgetStats inputGadget1, GadgetStats inputGadget2)
        {
            _gadgetSlots = FindAllGadgetSlots();
            
            comboButtonText.text = isCombo ? "UnCombine" : "Combine";
            
            _isCombo = isCombo;
            
            _inputGadget1 = inputGadget1;
            _inputGadget2 = inputGadget2;
            
            _selectedGadgetName = passedGadgetName;
            _selectedGadgetID = passedGadgetID;
            _selectedGadgetSprite = passedGadgetSprite;
            _selectedGadgetDescription = passedGadgetDescription;
            gadgetManagerCanvasGroup.interactable = false;
        }

        public void OnEquipButtonClicked()
        {
            foreach (IGadgetSlot gadgetSlot in _gadgetSlots)
            {
                gadgetSlot.ClearSlot(_selectedGadgetID);
            }
            
            if (equipSlot.hasEquipped)
            {
                Debug.Log("Already a gadget equipped, Unequipped");
                equipSlot.UnEquipInUI();
            }
            
            equipSlot.EquippedInUI(_selectedGadgetID, _selectedGadgetSprite, _selectedGadgetName, _selectedGadgetDescription, _isCombo, _inputGadget1, _inputGadget2);
            
            gadgetManagerCanvasGroup.interactable = true;
            gameObject.SetActive(false);
        }

        public void OnCombineButtonClicked()
        {
            if (!_isCombo)
            {
                foreach (CombineSlot combineSlot in combineSlots)
                {
                    if (combineSlot.isFull) continue;
                    combineSlot.AddToSlot(_selectedGadgetID, _selectedGadgetSprite,_selectedGadgetName, _selectedGadgetDescription, _isCombo, _inputGadget1, _inputGadget2);
                    foreach (IGadgetSlot gadgetSlot in _gadgetSlots)
                    {
                        gadgetSlot.ClearSlot(_selectedGadgetID);
                    }
                    break;
                }
            }

            else
            {
                InventoryManager.Instance.AddGadget(_inputGadget1.gadgetId, _inputGadget1.gadgetName, _inputGadget1.gadgetIcon, _inputGadget1.gadgetDescription, false, null, null);
                InventoryManager.Instance.AddGadget(_inputGadget2.gadgetId, _inputGadget2.gadgetName, _inputGadget2.gadgetIcon, _inputGadget2.gadgetDescription, false, null, null);
                
                foreach (IGadgetSlot gadgetSlot in _gadgetSlots)
                {
                    gadgetSlot.ClearSlot(_selectedGadgetID);
                }
                
            }
            
            gadgetManagerCanvasGroup.interactable = true;
            gameObject.SetActive(false);
        }

        public void OnCancelButtonClicked()
        {
            gadgetManagerCanvasGroup.interactable = true;
            gameObject.SetActive(false);
        }

        private List<IGadgetSlot> FindAllGadgetSlots()
        {
            IEnumerable<IGadgetSlot> slotObjects = 
                FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID).OfType<IGadgetSlot>();
            return new List<IGadgetSlot>(slotObjects);
        }
    }
}
