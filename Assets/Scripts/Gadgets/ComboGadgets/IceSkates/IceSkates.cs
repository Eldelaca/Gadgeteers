using UnityEngine;

namespace Gadgets.ComboGadgets.IceSkates
{
    public class IceSkates : MonoBehaviour
    {
        [SerializeField] MeshCollider waterCollider;
        [SerializeField] Respawner respawnScript;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) return;
            if (GadgetManager.Instance.equippedID != 9) return;
            waterCollider.isTrigger = false;
            respawnScript.enabled = false;
        }
    }
}
