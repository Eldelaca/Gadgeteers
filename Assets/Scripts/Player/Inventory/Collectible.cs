using UnityEngine;

namespace Player.Inventory
{
    public class Collectible : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            InventoryManager.Instance.AddCollectible();
            Destroy(gameObject);
        }
    }
}
