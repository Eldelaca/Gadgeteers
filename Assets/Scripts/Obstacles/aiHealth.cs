using UnityEngine;

public class aiHealth : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " took damage, health now: " + health);

        if (health <= 0f)
        {
            Debug.Log(gameObject.name + " died.");
            Destroy(gameObject);
        }
    }
}
