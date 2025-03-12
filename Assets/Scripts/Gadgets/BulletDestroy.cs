using System;
using Unity.VisualScripting;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);

        if (other.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
        
        else if (other.gameObject.CompareTag("AI"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    
}
