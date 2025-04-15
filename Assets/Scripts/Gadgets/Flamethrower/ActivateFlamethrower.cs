using UnityEngine;

public class ActivateFlamethrower : MonoBehaviour
{
    
    public GameObject flameCollider;  

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            flameCollider.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            flameCollider.SetActive(false);
        }
    }
}