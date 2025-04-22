using System.Collections.Generic;
using System.Linq;
using Gadgets;
using UnityEngine;

namespace UI.Inventory
{
    public class PopUpWindow : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup gadgetManagerCanvasGroup;
        [SerializeField] private CombineSlot[] comebineSlots;
        
        private int _selectedGadgetID;
        private Sprite _selectedGadgetSprite;
        private List<IGadgetSlot> _gadgetSlots;
        
        public void OnPopUpOpen(int passedGadgetID, Sprite passedGadgetSprite)
        {
            _gadgetSlots = FindAllGadgetSlots();
            
            _selectedGadgetID = passedGadgetID;
            _selectedGadgetSprite = passedGadgetSprite;
            gadgetManagerCanvasGroup.interactable = false;
        }

        public void OnEquipButtonClicked()
        {

            GadgetManager.Instance.OnEquip(_selectedGadgetID);


            foreach (IGadgetSlot gadgetSlot in _gadgetSlots)
            {
                gadgetSlot.ClearSlot(_selectedGadgetID);
            }
            
            gameObject.SetActive(false);
        }

        public void OnCombineButtonClicked()
        {
            foreach (CombineSlot combineSlot in comebineSlots)
            {
                if (combineSlot.isFull) continue;
                combineSlot.AddToSlot(_selectedGadgetID, _selectedGadgetSprite);
            }
            
            foreach (IGadgetSlot gadgetSlot in _gadgetSlots)
            {
                gadgetSlot.ClearSlot(_selectedGadgetID);
            }
            
            gameObject.SetActive(false);
        }

        public void OnCancelButtonClicked()
        {
            
            
            foreach (IGadgetSlot gadgetSlot in _gadgetSlots)
            {
                gadgetSlot.ClearSlot(_selectedGadgetID);
            }
            
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
