using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gadgets.BaseGadgets
{
    public class LightningWhip : MonoBehaviour, IGadget
    {
        [Header("References")] 
        [SerializeField] private GadgetStats stats;

        private bool _canSwing;
        private BoxCollider _collider;

        private void OnEnable()
        {
            _collider = GetComponent<BoxCollider>();
        }

        public void Equip()
        {
            if (stats.gadgetId != GadgetManager.Instance.equippedID) return;
            
            _canSwing = true;
            GadgetManager.Instance.lightningWhipEquipped = true; // Debugging only pwease remove after :3

        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            _canSwing = false;
            GadgetManager.Instance.bootsEquipped = false; // Debugging only pwease remove after :3
        }

        public void UseGadget()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            if (!_canSwing) return;
            Debug.Log("Using Lightning Whip");
            OnWhipSwing();
        }

        private void OnWhipSwing()
        {
            StartCoroutine(WhipDamageOverTime());
            _canSwing = false;
            StartCoroutine(WhipCooldown());
        }

        private IEnumerator WhipDamageOverTime()
        {
            _collider.enabled = true;
            yield return new WaitForSeconds(stats.useDuration);
            _collider.enabled = false;
        }
        
        private IEnumerator WhipCooldown()
        {
            yield return new WaitForSeconds(stats.useCooldown);
            _canSwing = true;
        }
    }
}
