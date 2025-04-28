using Gadgets;
using UnityEngine;

public class IceBullet : MonoBehaviour
{
    public GadgetStats IceGunStats;
    private void Start()
    {
        Debug.Log("Bullet initialised");
        
        // Destroy the bullet after lifeTime seconds
        Destroy(gameObject, IceGunStats.useDuration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) return;
        
        // If the bullet collides with an enemy tagged "AI"
        if (collision.collider.CompareTag("AI"))
        {
            aiHealth enemyHealth = collision.collider.GetComponent<aiHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(IceGunStats.gadgetMaxDamage);
            }
        }
        // Destroy the projectile on collision with any object
        Destroy(gameObject);
    }
}
