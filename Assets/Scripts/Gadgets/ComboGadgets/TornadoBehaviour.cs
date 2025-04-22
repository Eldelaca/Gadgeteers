// TornadoBehavior.cs
using UnityEngine;
using Gadgets;  // for GadgetStats

namespace Gadgets.ComboGadgets
{
    public class TornadoBehavior : MonoBehaviour
    {
        [Header("Fire Storm Stats")]
        public GadgetStats stats;

        
        float lifeTimer;

        // stats.gadgetDamageTick is being used as the lift force value 

        void Start()
        {
            lifeTimer = stats.useDuration;
        }

        void Update()
        {
            // move forward in local space
            transform.Translate(Vector3.forward * stats.range * Time.deltaTime, Space.Self);

            // lifespan countdown
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
                Destroy(gameObject);
        }

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
                rb.AddForce(Vector3.up * stats.gadgetDamageTick, ForceMode.Acceleration);
        }
    }
}
