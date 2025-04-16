using Gadgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This code handles the Flamethrower:
///     - Applies DoT damage via a trigger collider
///     - Responds to mouse input to toggle the flame effect on/off
/// </summary>
public class Flamethrower : MonoBehaviour
{
    public GadgetStats Flamestats;
    public GameObject flamethrowerCollider;


    // Keeps track of enemies/obstacles in a range
    private HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // Process only if the collider belongs to an enemy tagged "AI"
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
        // Remove enemy from tracking when it exits the collider
        if (enemiesInRange.Contains(other.gameObject))
        {
            Debug.Log("Enemy exited flame range: " + other.name);
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private IEnumerator DamageOverTime(GameObject enemy)
    {
        aiHealth enemyHealth = enemy.GetComponent<aiHealth>();

        // Continue to deal damage while enemy is in range and the collider is active
        while (enemiesInRange.Contains(enemy) && flamethrowerCollider.activeSelf)
        {
            if (enemyHealth != null)
            {
                Debug.Log("Dealing damage to: " + enemy.name);
                // Use the damage tick and max damage values from GadgetStats
                enemyHealth.TakeDamage(Flamestats.gadgetMaxDamage);
            }
            else
            {
                Debug.LogWarning("No aiHealth component found on " + enemy.name);
                yield break;
            }
            yield return new WaitForSeconds(Flamestats.gadgetDamageTick);
        }
    }
}
