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
        public GadgetStats IceGunStats; // 
        public GameObject IceBulletPrefab;  // Prefab of the ice projectile to instantiate
        private bool canShoot = true;
        private bool isEquipped = false;


        

        private void Update()
        {
            if (GadgetManager.Instance.equippedID != IceGunStats.gadgetId || !canShoot) return;  // Ensure that only equipped weapons can shoot

            
            if (Input.GetMouseButtonDown(0) && canShoot)
            {
                Shoot();
            }
        }

        // Handles instantiation of the projectile and applies force
        private void Shoot()
        {
            // Instantiate the ice bullet prefab at the gun's position and rotation
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
            isEquipped = true;  // Mark the gun as equipped

            GadgetManager.Instance.iceBlasterEquip = true;
        }   

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != IceGunStats.gadgetId) return;

            isEquipped = false;  // Mark the gun as unequipped
            GadgetManager.Instance.OnUnEquip();

            GadgetManager.Instance.iceBlasterEquip = false;
        }
    }
}
