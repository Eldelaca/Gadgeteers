using UnityEngine;

public class Respawner : MonoBehaviour
{

    public GameObject player;
    public GameObject respawnPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        { 

            player.transform.position = respawnPos.transform.position;

        }
    }

}
