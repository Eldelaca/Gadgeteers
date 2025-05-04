using Gadgets;
using UnityEngine;

public class Respawner : MonoBehaviour
{

    public GameObject player;
    public GameObject respawnPos;
    
    private BoxCollider _meshCollider;

    private void Awake()
    {
        _meshCollider = GetComponent<BoxCollider>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GadgetManager.Instance.equippedID == 9) return;
        player.transform.position = respawnPos.transform.position;
    }
}
