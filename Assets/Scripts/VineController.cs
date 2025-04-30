using System;
using UnityEngine;

public class VineController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name is "FlameThrower(Clone)" or "TornadoPrefab(Clone)" or "ExplosiveBoots(Clone)") Destroy(gameObject);
    }
}
