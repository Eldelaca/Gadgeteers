using Gadgets;
using UnityEngine;

public class WeaponToggle : MonoBehaviour
{
    public GadgetManager gadgetManager;

    private GameObject currentFlamethrower;
    private GameObject currentIceGun;

    public Transform playerHandle; 

    private void Update()
    {
        // === Flamethrower ===
        if (gadgetManager.flamethrowerEquipped)
        {
            if (currentFlamethrower == null)
            {
                gadgetManager._equippedID = 1;

                currentFlamethrower = Instantiate(gadgetManager.baseGadgets[0], playerHandle.position, playerHandle.rotation);
                currentFlamethrower.transform.parent = playerHandle;
                currentFlamethrower.SetActive(false); // Start inactive
                Debug.Log("Flamethrower instantiated.");
            }

            if (Input.GetMouseButtonDown(0))
            {
                currentFlamethrower.SetActive(true);
                Debug.Log("Flamethrower activated.");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                currentFlamethrower.SetActive(false);
                Debug.Log("Flamethrower deactivated.");
            }
        }
        else
        {
            if (currentFlamethrower != null)
            {
                Destroy(currentFlamethrower);
                currentFlamethrower = null;
                Debug.Log("Flamethrower destroyed.");
            }
        }

        // === Ice Gun ===
        if (gadgetManager.iceBlasterEquip)
        {
            if (currentIceGun == null)
            {
                gadgetManager._equippedID = 2;

                currentIceGun = Instantiate(gadgetManager.baseGadgets[1], playerHandle.position, playerHandle.rotation);
                currentIceGun.transform.parent = playerHandle;
                Debug.Log("Ice Gun instantiated.");
            }

            currentIceGun.SetActive(true);
        }
        else
        {
            if (currentIceGun != null)
            {
                Destroy(currentIceGun);
                currentIceGun = null;
                Debug.Log("Ice Gun destroyed.");
            }
        }
    }
}
