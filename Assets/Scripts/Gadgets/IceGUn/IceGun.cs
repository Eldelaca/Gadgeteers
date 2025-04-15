using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// This code handles the IceGun:
///     - Projectile damage
///     - Mouse Inputs
///     - Instantiating the projectile Prefab with force
///     - Enforcing a cooldown between shots
/// </summary>
public class IceGun : MonoBehaviour
{
    [Header("Gun Settings")]
    public GameObject IceBulletPrefab;  // Prefab of the ice projectile to instantiate
    public float IceDamage = 10f;         // Damage dealt by the projectile (passed to the bullet)
    public float shootingCooldown = 1f;   // Cooldown in seconds before next shot

    [Header("Bullet Settings")]
    public float bulletForce = 500f;      // Force applied to the projectile

    // Flag to ensure shooting only happens after cooldown
    private bool canShoot = true;

    private void Update()
    {
        // Listen for mouse input (left click) and check if gun is ready to fire
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Shoot();
        }
    }

    // Handles instantiation of the projectile and applies force
    private void Shoot()
    {
        
        GameObject iceBullet = Instantiate(IceBulletPrefab, transform.position, transform.rotation);

        
        IceBullet bulletScript = iceBullet.GetComponent<IceBullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = IceDamage;
        }

        // Apply force to the bullet if it has a Rigidbody component
        Rigidbody rb = iceBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Add a force in the forward direction of this gun
            rb.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
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
        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true;
    }
}
