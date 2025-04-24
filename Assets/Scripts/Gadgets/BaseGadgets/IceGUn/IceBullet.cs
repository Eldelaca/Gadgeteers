using Gadgets;
using UnityEngine;

public class IceBullet : MonoBehaviour
{
    public GadgetStats IceGunStats;
    private void Start()
    {
        // Destroy the bullet after lifeTime seconds
        Destroy(gameObject, IceGunStats.useDuration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the bullet collides into enemy tagged "AI"
        // CHANGE TAG FOR SPECIFIC OBSTACLES

        if (collision.collider.CompareTag("AI"))
        {
            aiHealth enemyHealth = collision.collider.GetComponent<aiHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(IceGunStats.gadgetMaxDamage);
            }
        }
        // Destroy the projectile on collision with any object (If it hits the floor)
        Destroy(gameObject);
    }
}
