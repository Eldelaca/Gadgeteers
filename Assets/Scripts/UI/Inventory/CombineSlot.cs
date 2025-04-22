using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class CombineSlot : MonoBehaviour
    {
        public bool isFull;

        [SerializeField] private Sprite defaultSprite;
        
        private Image _image;
        private Button _button;
        private int _storedID;

        private void OnEnable()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _image.sprite = defaultSprite;
            _button.interactable = false;
        }
        
        public void AddToSlot(int gadgetID, Sprite gadgetSprite)
        {
            _storedID = gadgetID;
            
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.interactable = true;
            _image.sprite = gadgetSprite;
            
            isFull = true;
        }

        public void ClearSlot()
        {
            _storedID = 0;
            
            _image.sprite = defaultSprite;
            _button.interactable = false;
            
        }
    }
}
