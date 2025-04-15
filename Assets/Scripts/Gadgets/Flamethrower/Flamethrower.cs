using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This code handles the Flamethrower code
///     - DoT damage
///     - Mouse Inputs
/// </summary>
public class Flamethrower : MonoBehaviour
{
    public GameObject flamethrowerCollider;
    public float DoTdamage = 10f; // Can be changed 
    public float DoTRate = 1.5f;  // The rate at which it takes damage over time

    // This should keep track of the enemies/obstacles in the game
    private HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();



    private void OnTriggerEnter(Collider other)
    {
        // This code should activate only when the enemy/obstacle enters the collider
        if (!other.CompareTag("AI")) return;

        if (!enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Add(other.gameObject);
            Debug.Log("Enemy entered flame range: " + other.name);

            StartCoroutine(DamageOverTime(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Something entered: " + other.name);

        // This code should remove the enemy from tracking when it exits the collider
        if (enemiesInRange.Contains(other.gameObject))
        {
            Debug.Log("Enemy exited flame range: " + other.name);
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private IEnumerator DamageOverTime(GameObject enemy)
    {
        aiHealth enemyHealth = enemy.GetComponent<aiHealth>();

        // This coroutine runs while the enemy is in range (Collider)
        // and if the flamethrower is active
        while (enemiesInRange.Contains(enemy) && flamethrowerCollider.activeSelf)
        {
            if (enemyHealth != null)
            {
                Debug.Log("Dealing damage to: " + enemy.name);
                enemyHealth.TakeDamage(DoTdamage);
            }
            else
            {
                Debug.LogWarning("No aiHealth component found on " + enemy.name);
                yield break;
            }
            yield return new WaitForSeconds(DoTRate);
        }
    }
}
