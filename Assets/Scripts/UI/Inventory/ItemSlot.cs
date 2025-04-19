using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI.Inventory
{
    public class ItemSlot : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image image;

        private int _gadgetID;
        private string _gadgetName;
        private Sprite _gadgetSprite;
        
        public bool slotFull;
        
        public void FillSlot(int gadgetId, string gadgetName, Sprite gadgetSprite)
        {
            _gadgetID = gadgetId;
            _gadgetName = gadgetName;
            _gadgetSprite = gadgetSprite;
            
            slotFull = true;
            
            nameText.text = gadgetName;
            nameText.enabled = true;
            image.sprite = gadgetSprite;
        }
    }
}
