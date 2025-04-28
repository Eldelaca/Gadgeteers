using Gadgets;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI.Inventory
{
    public class ItemSlot : MonoBehaviour, IGadgetSlot
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image image;
        [SerializeField] private GameObject popUp;
        [SerializeField] private GameObject selectionPanel;
        [SerializeField] private PopUpWindow popUpWindowScript;
        [SerializeField] private Sprite defaultSprite;

        private int _gadgetID;
        private Sprite _gadgetSprite;
        private string _gadgetName, _gadgetDescription;
        private bool _isCombo;
        private GadgetStats _inputGadget1, _inputGadget2;

        
        public bool slotFull;
        
        public void FillSlot(int gadgetId, string gadgetName, Sprite gadgetSprite, string gadgetDescription, bool isCombo, GadgetStats input1, GadgetStats input2)
        {
            _gadgetID = gadgetId;
            _gadgetSprite = gadgetSprite;
            _gadgetDescription = gadgetDescription;
            
            _isCombo = isCombo;
            
            _inputGadget1 = input1;
            _inputGadget2 = input2;
            
            slotFull = true;
            
            nameText.text = gadgetName;
            image.sprite = gadgetSprite;
        }

        public void OnSlotClicked()
        {
            if (!slotFull) return;
            
            selectionPanel.SetActive(true);
            
            popUp.SetActive(true);
            popUp.transform.position = image.transform.position;

            popUpWindowScript.OnPopUpOpen(_gadgetID, _gadgetSprite, _gadgetName, _gadgetDescription, _isCombo, _inputGadget1, _inputGadget2);

        }
        
        public void DeselectSlot(int gadgetID)
        {
            if (gadgetID != _gadgetID) return;
            selectionPanel.SetActive(false);
        }

        public void ClearSlot(int gadgetID)
        {
            if (gadgetID != _gadgetID) return;
            
            nameText.enabled = false;
            image.sprite = defaultSprite;
            _gadgetID = 0;
            
            slotFull = false;
        }
    }
}
