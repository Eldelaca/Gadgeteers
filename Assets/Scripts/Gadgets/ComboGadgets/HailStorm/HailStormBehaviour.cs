// TornadoBehavior.cs
using UnityEngine;
using Gadgets;  // for GadgetStats


/// <summary>
/// This Code can be interchangable to fit any other Storms we have Ice/Fire/Electric/Wind(Normal)
/// This Code Happens why an Object other than player collides with the Object
/// Uses the Gadget Stats Script
/// </summary>



namespace Gadgets.ComboGadgets
{
    public class HailStormBehavior : MonoBehaviour
    {
        [Header("Fire Storm Stats")]
        public ComboGadgetStats stats;


        float lifeTimer;

        // ****** NOTICE ******  // 
        // stats.gravityMultiplier is being used as the lift force value 

        void Start()
        {
            lifeTimer = stats.useDuration;
        }

        void Update()
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

           
        }


        // This is where what happens in the level
        void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("AI")) return;

            // damage over time
            var hp = other.GetComponent<aiHealth>();
            if (hp != null)
                hp.TakeDamage(stats.gadgetMaxDamage * Time.deltaTime);

            // lift upward
            var rb = other.attachedRigidbody;
            if (rb != null)
                rb.AddForce(Vector3.up * stats.gravityMultiplier, ForceMode.Acceleration);
        }
    }
}
