using System;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    Rigidbody[] Rb;

    void Start()
    {
        Rb = GetComponentsInChildren<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FallApart();
        }
    }

    private void FallApart()
    {
        foreach (Rigidbody rb in Rb)
        {
            rb.isKinematic = false;
        }
        Destroy(gameObject, 20);
    }
    
}
