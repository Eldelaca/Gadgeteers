using Gadgets;
using Player.Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class CombineSlot : MonoBehaviour
    {
        public bool isFull;

        [SerializeField] private Sprite defaultSprite;
        
        private Image _image;
        private Button _button;
        public int storedID;
        private string _gadgetName, _gadgetDescription;
        private Sprite _gadgetSprite;
        private GadgetStats _inputGadget1, _inputGadget2;

        private bool _isCombo;

        private void OnEnable()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _image.sprite = null;
            _button.interactable = false;
        }
        
        public void AddToSlot(int gadgetID, Sprite gadgetSprite, string gadgetName, string gadgetDescription, bool isCombo, GadgetStats inputGadget1, GadgetStats inputGadget2)
        {
            storedID = gadgetID;
            _gadgetName = gadgetName;
            _gadgetDescription = gadgetDescription;
            _gadgetSprite = gadgetSprite;
            
            _isCombo = isCombo;
            
            _inputGadget1 = inputGadget1;
            _inputGadget2 = inputGadget2;
            
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.interactable = true;
            _image.sprite = gadgetSprite;
            
            isFull = true;
        }

        public void ClearSlot()
        {
            InventoryManager.Instance.AddGadget(storedID, _gadgetName, _gadgetSprite, _gadgetDescription, _isCombo, _inputGadget1, _inputGadget2);

            storedID = 0;
            
            _image.sprite = null;
            _button.interactable = false;
            
            isFull = false;
        }

        public void EmptySlot()
        {
            _image.sprite = null;
            _button.interactable = false;
            
            isFull = false;
        }
    }
}
