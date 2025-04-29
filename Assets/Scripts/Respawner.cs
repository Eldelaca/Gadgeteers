using Gadgets;
using UnityEngine;

public class Respawner : MonoBehaviour
{

    public GameObject player;
    public GameObject respawnPos;
    
    private MeshCollider _meshCollider;

    private void Awake()
    {
        _meshCollider = GetComponent<MeshCollider>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(GadgetManager.Instance.equippedID);
            if (GadgetManager.Instance.equippedID == 9)
            {
                _meshCollider.isTrigger = false;
                return;
            }
            player.transform.position = respawnPos.transform.position;
        }
    }
}
