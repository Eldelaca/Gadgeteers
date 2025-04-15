using UnityEngine;

// This code activates the flamethrower

public class ActivateWeapons : MonoBehaviour
{
    // Use a Bool value to active the weapons
    public GameObject flamethrower;  
    public bool Flamethrower = false;



    private void Update()
    {
        // Flamethrower
        if (Flamethrower)
        {
            if (Input.GetMouseButtonDown(0))
            {
                flamethrower.SetActive(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                flamethrower.SetActive(false);
            }
        }
        
    }
}