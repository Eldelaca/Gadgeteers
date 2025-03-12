using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Player") && other.CompareTag("Coin"))
        {
            PickupCoin(other.gameObject);
        }
    }

    void PickupCoin(GameObject coin)
    {
        Destroy(coin); // Remove the coin from the scene
    }
}