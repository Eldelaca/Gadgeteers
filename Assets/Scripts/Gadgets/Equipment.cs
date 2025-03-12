using Unity.VisualScripting;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    // Ref Scripts
    public CharacterController characterControllerScript;
    public GameObject iceGunScript;

    // Ref Items

    public bool _hasBoots = false; // Handles Double Jump
    public bool _hasIceGun = false; // Fires Ice Damage Gun
    public bool _hasWhip = false;
    public bool _hasFireGun = false;

    private void Start()
    {
        iceGunScript.SetActive(false);
    }


    // Ice Combos

    // ********** Ice Gun ********** 
    public void IceGun()
    {
        if (_hasIceGun)
        {
            iceGunScript.SetActive(true);
        }
        else
        {
            iceGunScript.SetActive(false);
        }
    }

    // ********** Ice Boots Variant ********** 
    // Collision based when the player is colliding with the other tags allowing to walk over
    void OnTriggerEnter(Collider other)
    {
        // Water Tag Compare
        if (other.gameObject.CompareTag("Water"))
        {
            Collider waterCollider = other.GetComponent<Collider>();

            if (_hasBoots && _hasIceGun)
            {
                waterCollider.isTrigger = false; // Make the water solid
                Debug.Log("Walking on water with Ice Boots.");
            }
            else
            {
                waterCollider.isTrigger = true;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        Collider waterCollider = collision.gameObject.GetComponent<Collider>();
        if (collision.gameObject.CompareTag("Water"))
        {
            if (_hasBoots && _hasIceGun)
            {
                waterCollider.isTrigger = false; // Make the water solid
                Debug.Log("Walking on water with Ice Boots.");
            }
            else
            {
                waterCollider.isTrigger = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
        {
        // Once Exited
        if (collision.gameObject.CompareTag("Water"))
        {
            Collider waterCollider = collision.gameObject.GetComponent<Collider>();

            waterCollider.isTrigger = true; // Reset the water back to a trigger
            Debug.Log("Exited water.");
        }
    }

    // ********** Ice Boots Variant ********** 
}
