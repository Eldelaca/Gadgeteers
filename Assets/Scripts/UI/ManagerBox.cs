using System;
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class ManagerBox : MonoBehaviour, IBoxControls
    {
        private string _uniqueId;
        private BoxCollider _boxCollider;

        private void Awake()
        {
            _uniqueId = Guid.NewGuid().ToString();
            _boxCollider = GetComponent<BoxCollider>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            GadgetManagerUI.Instance.OpenUI(_uniqueId);
        }
        
        public void ClearBox(string uniqueID, bool changedGadget)
        {
            if (uniqueID != _uniqueId) return;
            if (changedGadget)
            {
                Destroy(gameObject);
            }

            _boxCollider.enabled = false;
            StartCoroutine(ColliderSwitch());
        }

        private IEnumerator ColliderSwitch()
        {
            yield return new WaitForSeconds(2f);
            _boxCollider.enabled = true;
        }
    }
}
