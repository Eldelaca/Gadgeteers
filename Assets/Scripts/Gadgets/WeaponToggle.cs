using Gadgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActiveFlameToggle : MonoBehaviour
{
    public GameObject flamethrowerCollider;
    public GameObject IceGun;

    public GadgetManager gadgetManager;
    private void Update()
    {

    // Flamethrower
        // Check if the flamethrower gadget is equipped
        if (gadgetManager.flamethrowerEquipped)
        {
            // Even if flamethrowerCollider is inactive, we can set it active via input
            if (Input.GetMouseButtonDown(0))
            {
                flamethrowerCollider.SetActive(true);
                Debug.Log("?? Flamethrower activated");
                Debug.Log("Flamethrower activated");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                flamethrowerCollider.SetActive(false);
                Debug.Log("Flamethrower deactivated");
            }
        }
    // IceGun

        if (gadgetManager.iceBlasterEquip)
        {
            IceGun.SetActive(true);
        }
        else
        {
            IceGun.SetActive(false);
        }
    }
}