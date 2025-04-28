using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    Rigidbody[] Rb;

    void Start()
    {
        Rb = GetComponentsInChildren<Rigidbody>();
    }

    public void FallApart()
    {

        foreach (Rigidbody rb in Rb)
        {
            rb.isKinematic = false;
        }
        Destroy(gameObject, 20);
    }
    
}
