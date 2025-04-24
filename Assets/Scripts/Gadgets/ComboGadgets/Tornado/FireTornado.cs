// FireTornadoGadget.cs
using UnityEngine;
using Gadgets;                     // for GadgetStats & GadgetManager
using System.Collections;


/// <summary>
/// This Code can be interchangable as it uses the same logic for spawning torndaos/ Storms
/// Responsible for handling the spawns of the Tornado GameObject
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
            if (GadgetManager.Instance.equippedID != tornadoStats.gadgetId || isOnCooldown) return;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Left mouse button clicked");
                SpawnTornado();
                StartCoroutine(CooldownCoroutine());
            }
        }


        // Code that spawns the Torando Prefab
        private void SpawnTornado()
        {
            try
            {
                if (tornadoPrefab == null)
                {
                    Debug.LogError("Tornado prefab is null!");
                    return;
                }

                Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up;
                var t = Instantiate(tornadoPrefab, spawnPos, Quaternion.LookRotation(transform.forward));

                var beh = t.GetComponent<TornadoBehavior>();
                if (beh != null)
                {
                    beh.stats = tornadoStats;
                }
                else
                {
                    Debug.LogWarning("TornadoBehavior script missing on prefab!");
                }

                Debug.Log("Fire Tornado spawned.");
            }
            catch (System.Exception e)
            {
                Debug.LogError(" Exception spawning tornado: " + e.Message);
            }
        }


        private IEnumerator CooldownCoroutine()
        {
            isOnCooldown = true;
            yield return new WaitForSeconds(tornadoStats.useCooldown);
            isOnCooldown = false;
        }

        // IGadget
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
