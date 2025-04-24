using UnityEngine;
using System.Collections;
using Gadgets;

namespace Gadgets.BaseGadgets
{
    /// <summary>
    /// This code handles the IceGun:
    ///     - Projectile damage
    ///     - Mouse Inputs
    ///     - Instantiating the projectile Prefab with force
    ///     - Enforcing a cooldown between shots
    /// </summary>
    public class IceGun : MonoBehaviour, IGadget
    {
        [Header("Gun Settings")]
        public GadgetStats IceGunStats; // For Gadget Stats
        public GameObject IceBulletPrefab;  // Prefab of the ice projectile to instantiate

        private bool canShoot = true; // Checks if it can shoot 


        

        private void Update()
        {
            // Ensure that only equipped weapons can shoot
            // Not really needed but just in case the game doesnt think this weapon is equipped
            // Dont shoot
            if (GadgetManager.Instance.equippedID != IceGunStats.gadgetId || !canShoot) return;

            
            if (Input.GetMouseButtonDown(0) && canShoot)
            {
                Shoot();
            }
        }

        // Handles instantiation of the projectile and applies force
        private void Shoot()
        {
            // Makes sure to spawn them in the position of the gun
            // This can change if we have a model for the gun
            GameObject iceBullet = Instantiate(IceBulletPrefab, transform.position, transform.rotation);

            // Apply force to the bullet with RigidBody
            Rigidbody rb = iceBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Add force in the forward direction of the Ice Gun
                rb.AddForce(transform.forward * IceGunStats.range, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("IceBullet prefab is missing a Rigidbody component.");
            }

            // Disable shooting and start cooldown
            canShoot = false;
            StartCoroutine(ShootingCooldown());
        }

        // Coroutine for handling the shooting cooldown
        private IEnumerator ShootingCooldown()
        {
            yield return new WaitForSeconds(IceGunStats.useCooldown);
            canShoot = true;
        }

        // Using IGadget for Equipping
        public void Equip()
        {
            if (GadgetManager.Instance.equippedID == IceGunStats.gadgetId)
            {
                Debug.Log("Ice Gun already equipped");
                return;
            }

            if (GadgetManager.Instance.equippedID != 0)
            {
                GadgetManager.Instance.OnUnEquip();
            }

            GadgetManager.Instance.OnEquip(IceGunStats.gadgetId);
            

            GadgetManager.Instance.iceBlasterEquip = true;
        }   

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != IceGunStats.gadgetId) return;

            GadgetManager.Instance.OnUnEquip();

            GadgetManager.Instance.iceBlasterEquip = false;
        }
    }
}
