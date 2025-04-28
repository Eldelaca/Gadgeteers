using Gadgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// This code handles the Flamethrower:
///     - Applies DoT damage via a trigger collider
///     - Responds to mouse input to toggle the flame effect on/off
/// </summary>

namespace Gadgets.BaseGadgets
{
    public class Flamethrower : MonoBehaviour, IGadget
    {

        // Variables for GameObjects its Connected to
        public GadgetStats Flamestats;
        public GameObject flamethrowerCollider;
        public VisualEffect flameEffect;

        // Keeps track of enemies/obstacles in a range
        private HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();

        private BoxCollider boxCollider;

        private void Start()
        {
            // Checking if has Box Collider Component
            if (flamethrowerCollider != null) 
            {
                boxCollider = flamethrowerCollider.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    // Make sure its off at start
                    boxCollider.enabled = false;
                }
                else
                {
                    Debug.LogWarning("BoxCollider not found on flamethrowerCollider.");
                }
            }
        }

        private void Update()
        {
            if (boxCollider == null) return;
            
            // This doesnt Uses the old Input System
            if (Input.GetMouseButton(0))
            {
                boxCollider.enabled = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                boxCollider.enabled = false;
            }
        }

        #region Action Methods
========
        private HashSet<GameObject> enemiesInRange;

        private CapsuleCollider boxCollider;
        
        public void Equip()
        {
            if (GadgetManager.Instance.equippedID != Flamestats.gadgetId) return;
            
            flamethrowerCollider.SetActive(true);
            boxCollider = flamethrowerCollider.GetComponent<CapsuleCollider>();
        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != Flamestats.gadgetId) return;

            flamethrowerCollider.SetActive(false);
        }

        public void UseGadget()
        {
            if (GadgetManager.Instance.equippedID != Flamestats.gadgetId) return;
            
            boxCollider.enabled = !boxCollider.enabled;
            flameEffect.enabled = !flameEffect.enabled;
        }

>>>>>>>> origin/ciaranold:Assets/Scripts/Gadgets/BaseGadgets/Flamethrower.cs
        private void OnTriggerEnter(Collider other)
        {
            // Process only if the collider belongs to an enemy tagged "AI"
            if (!other.CompareTag("AI")) return;

            if (!enemiesInRange.Add(other.gameObject)) return;
            Debug.Log("Enemy entered flame range: " + other.name);
            StartCoroutine(DamageOverTime(other.gameObject));
        }

        private void OnTriggerExit(Collider other)
        {
            // Remove enemy from tracking when it exits the collider
            if (!enemiesInRange.Contains(other.gameObject)) return;
            Debug.Log("Enemy exited flame range: " + other.name);
            enemiesInRange.Remove(other.gameObject);
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
        #endregion

        #region Equip Methods
        public void Equip()
        {
            if (GadgetManager.Instance.equippedID == Flamestats.gadgetId)
            {
                Debug.Log("Flamethrower already equipped");
                return;
            }

            if (GadgetManager.Instance.equippedID != 0)
            {
                GadgetManager.Instance.OnUnEquip();
            }

            GadgetManager.Instance.OnEquip(Flamestats.gadgetId);
            flamethrowerCollider.SetActive(true);

            GadgetManager.Instance.flamethrowerEquipped = true;

        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != Flamestats.gadgetId) return;

            flamethrowerCollider.SetActive(false);
            GadgetManager.Instance.OnUnEquip();

            GadgetManager.Instance.flamethrowerEquipped = false;

        }

        #endregion
    }


}
