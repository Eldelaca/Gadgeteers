using UnityEngine;

public class Pickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collect"))
        {
            Destroy(other.gameObject);
        }
    }
}
