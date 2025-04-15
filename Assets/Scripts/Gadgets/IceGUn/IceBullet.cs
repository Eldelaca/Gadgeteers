using UnityEngine;

public class IceBullet : MonoBehaviour
{
    public float damage;          
    public float lifeTime = 3f;   

    private void Start()
    {
        // Destroy the bullet after lifeTime seconds
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the bullet collides with an enemy tagged "AI"
        if (collision.collider.CompareTag("AI"))
        {
            aiHealth enemyHealth = collision.collider.GetComponent<aiHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
        // Destroy the projectile on collision with any object
        Destroy(gameObject);
    }
}
