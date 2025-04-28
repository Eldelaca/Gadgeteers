using System;
using UnityEngine;

public class VineController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FlameThrower(Clone)") Destroy(gameObject);
    }
}
