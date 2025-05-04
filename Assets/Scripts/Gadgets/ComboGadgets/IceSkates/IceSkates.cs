using System;
using System.Collections;
using UnityEngine;

namespace Gadgets.ComboGadgets.IceSkates
{
    public class IceSkates : MonoBehaviour
    {
        [SerializeField] private MeshCollider waterCollider;
        [SerializeField] private Respawner respawnScript;
        [SerializeField] private GameObject feetLocation;
        [SerializeField] private GameObject iceSheet;

        private bool _canPlace = true;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Water")) return;
            other.isTrigger = false;
            Instantiate(iceSheet, gameObject.transform.position, Quaternion.identity);
            _canPlace = false;

            StartCoroutine(IceSkateCooldown());
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_canPlace) return;
            if (!other.gameObject.CompareTag("Water")) return;
            other.isTrigger = false;
            Instantiate(iceSheet, gameObject.transform.position, Quaternion.identity);
            _canPlace = false;

            StartCoroutine(IceSkateCooldown());


        }

        private IEnumerator IceSkateCooldown()
        {
            yield return new WaitForSeconds(0.3f);
            _canPlace = true;
        }
    }
}
