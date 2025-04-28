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
        public GadgetStats tornadoStats;
        public GameObject tornadoPrefab;

        bool isOnCooldown = false;

        void Update()
        {
            if (isOnCooldown)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                SpawnTornado();
                StartCoroutine(CooldownCoroutine());
            }
        }

        private void SpawnTornado()
        {
            try
            {
                if (tornadoPrefab == null)
                {
                    Debug.LogError("Tornado prefab is null!");
                    return;
                }

                // Spawn at player position
                Vector3 spawnPos = transform.position;
                var tornado = Instantiate(
                    tornadoPrefab,
                    spawnPos,
                    Quaternion.LookRotation(transform.forward)
                );

                // Setup behavior to follow this transform
                var beh = tornado.GetComponent<TornadoBehavior>();
                if (beh != null)
                {
                    beh.stats = tornadoStats;
                    beh.followTarget = this.transform;  // assign follow target
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
            if (GadgetManager.Instance.equippedID != 0)
                GadgetManager.Instance.OnUnEquip();

            GadgetManager.Instance.OnEquip(tornadoStats.gadgetId);
            GadgetManager.Instance.flamethrowerEquipped = true;
            GadgetManager.Instance.lightningWhipEquipped = true;
        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != tornadoStats.gadgetId) return;

            GadgetManager.Instance.OnUnEquip();
            GadgetManager.Instance.flamethrowerEquipped = false;
            GadgetManager.Instance.lightningWhipEquipped = false;
        }
    }
}
