using System;
using Gadgets;
using Player.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class EquipSlot : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] Button unequipButton;
        public bool hasEquipped;

        private int _equippedGadgetID;
        private Sprite _equippedGadgetSprite;
        private string _equippedGadgetName, _equippedGadgetDescription;
        private bool _isCombo;
        private GadgetStats _gadgetInput1, _gadgetInput2;
        
        public void EquippedInUI(int gadgetID, Sprite gadgetSprite,string gadgetName, string gadgetDescription, bool isCombo, GadgetStats gadgetInput1, GadgetStats gadgetInput2)
        {
            _equippedGadgetID = gadgetID;
            _equippedGadgetSprite = gadgetSprite;
            _equippedGadgetName = gadgetName;
            _equippedGadgetDescription = gadgetDescription;
            
            _gadgetInput1 = gadgetInput1;
            _gadgetInput2 = gadgetInput2;

            _isCombo = isCombo;
            
            GadgetManager.Instance.OnEquip(gadgetID);
            
            descriptionText.text = $"{gadgetName}{Environment.NewLine}{Environment.NewLine}{gadgetDescription}";
            
            image.sprite = gadgetSprite;
            hasEquipped = true;
            
            unequipButton.interactable = true;
        }

        public void UnEquipInUI()
        {
            InventoryManager.Instance.AddGadget(_equippedGadgetID,  _equippedGadgetName, _equippedGadgetSprite, _equippedGadgetDescription, _isCombo, _gadgetInput1, _gadgetInput2);
            
            GadgetManager.Instance.OnUnEquip();
            
            _equippedGadgetID = 0;
            _equippedGadgetName = null;
            _equippedGadgetDescription = null;
            _equippedGadgetSprite = null;
            
            descriptionText.text = "";
             
            image.sprite = null;
            hasEquipped = false;
            
            unequipButton.interactable = false;
        }
    }
}
