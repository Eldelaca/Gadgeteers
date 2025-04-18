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

        private void Awake()
        {
            _uniqueId = Guid.NewGuid().ToString();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            GadgetManagerUI.Instance.OpenUI(_uniqueId);
        }
        
        public void ClearBox(string uniqueID)
        {
            if (uniqueID != _uniqueId) return;

            Destroy(gameObject);
            // StartCoroutine(ScaleOverTime());
        }

        /*private IEnumerator ScaleOverTime()
        {
            
        }*/
    }
}
