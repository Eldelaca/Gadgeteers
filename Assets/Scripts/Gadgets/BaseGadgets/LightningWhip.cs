using UnityEngine;

namespace Gadgets.BaseGadgets
{
    public class LightningWhip : MonoBehaviour, IGadget
    {
        [Header("References")]
        [SerializeField] private GadgetStats stats;
        [SerializeField] private GameObject playerHandle;

        private GameObject _lightningWhipGameObject;

        private bool _isEquipped;
        
        public void Equip()
        {
            if (stats.gadgetId != GadgetManager.Instance.equippedID) return;
            _isEquipped = true;
            
            Transform lightningWhipTransform = playerHandle.transform.Find("LightningWhip");
            _lightningWhipGameObject = lightningWhipTransform.gameObject;
            
            GadgetManager.Instance.lightningWhipEquipped = true; // Debugging only pwease remove after :3
            
        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;
            _isEquipped = false;
            
            _lightningWhipGameObject = null;
            
            GadgetManager.Instance.bootsEquipped = false; // Debugging only pwease remove after :3
        }

        public void OnWhipSwing()
        {
            if (!_isEquipped) return;
            
            
        }
    }
}
