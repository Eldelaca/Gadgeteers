// FireTornadoGadget.cs
using UnityEngine;
using Gadgets;                     // for GadgetStats & GadgetManager
using System.Collections;


/// <summary>
/// Spawns a tornado that follows the player using a followTarget pattern.
/// </summary>

namespace Gadgets.ComboGadgets
{
    public class FireTornado : MonoBehaviour, IGadget
    {
        [Header("Stats & Prefab")]
        public ComboGadgetStats tornadoStats;
        public GameObject tornadoPrefab;

        private GameObject activeTornado;
        bool isOnCooldown = false;

        private void SpawnTornado()
        {
            try
            {
                // Checking if it grabs the Tornado Script
                if (tornadoPrefab == null)
                {
                    Debug.LogError("Tornado prefab is null!");
                    return;
                }

                // Checking if the Tornado is active
                if (activeTornado != null)
                {
                    Debug.Log("Tornado already active, not spawning a new one.");
                    return;
                }

                // instantiates at player
                Vector3 spawnPos = transform.position;
                activeTornado = Instantiate(
                            tornadoPrefab,
                            spawnPos,
                            Quaternion.LookRotation(transform.forward)
                );

                
                var beh = activeTornado.GetComponent<TornadoBehavior>();
                if (beh != null)
                {
                    beh.stats = tornadoStats;
                    beh.followTarget = this.transform; 
                }
                else
                {
                    Debug.LogWarning("TornadoBehavior script missing on prefab!");
                }

                Debug.Log("Fire Tornado spawned and set to follow player.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exception spawning tornado: " + e.Message);
            }
        }

        private IEnumerator CooldownCoroutine()
        {
            isOnCooldown = true;
            yield return new WaitForSeconds(tornadoStats.useCooldown);
            isOnCooldown = false;
        }

        public void Equip()
        {
            if (GadgetManager.Instance.equippedID == tornadoStats.gadgetId) return;

            GadgetManager.Instance.flamethrowerEquipped = true;
            GadgetManager.Instance.lightningWhipEquipped = true;
        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != tornadoStats.gadgetId) return;

            GadgetManager.Instance.flamethrowerEquipped = false;
            GadgetManager.Instance.lightningWhipEquipped = false;
        }

        public void UseGadget()
        {
            if (GadgetManager.Instance.equippedID != tornadoStats.gadgetId) return;
            
            if (isOnCooldown)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                SpawnTornado();
                StartCoroutine(CooldownCoroutine());
            }
        }
    }
}
