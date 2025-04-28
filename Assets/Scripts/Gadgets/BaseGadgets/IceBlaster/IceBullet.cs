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
<<<<<<<< HEAD:Assets/Scripts/Gadgets/BaseGadgets/IceGUn/IceBullet.cs
        // If the bullet collides into enemy tagged "AI"
        // CHANGE TAG FOR SPECIFIC OBSTACLES

========
        if (collision.collider.CompareTag("Player")) return;
        
        // If the bullet collides with an enemy tagged "AI"
>>>>>>>> origin/ciaranold:Assets/Scripts/Gadgets/BaseGadgets/IceBlaster/IceBullet.cs
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
