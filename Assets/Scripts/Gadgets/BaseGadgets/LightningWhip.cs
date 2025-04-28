using UnityEngine;

namespace Gadgets.BaseGadgets
{
    public class LightningWhip : MonoBehaviour, IGadget
    {
        [Header("References")] [SerializeField]
        private GadgetStats stats;

        [SerializeField] private GameObject playerHandle;
        [SerializeField] private LayerMask whatIsGrapple;
        [SerializeField] private CharacterController player;

        [Header("Grapple Settings")] [SerializeField]
        private float maxDistance = 1000f;

        [SerializeField] private float grappleSpeed = 20f;

        private LineRenderer _lr;

        private Vector3 _grapplePoint;
        private bool _isGrappling;

        /*private GameObject _lightningWhipGameObject;*/


        private bool _canSwing;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _lr = GetComponent<LineRenderer>();
        }

        public void Equip()
        {
            if (stats.gadgetId != GadgetManager.Instance.equippedID) return;

            /*Transform lightningWhipTransform = playerHandle.transform.Find("LightningWhip");
            _lightningWhipGameObject = lightningWhipTransform.gameObject;*/

            GadgetManager.Instance.lightningWhipEquipped = true; // Debugging only pwease remove after :3

        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            // _lightningWhipGameObject = null;

            GadgetManager.Instance.bootsEquipped = false; // Debugging only pwease remove after :3
        }

        public void UseGadget()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            if (!_canSwing) return;
            OnWhipSwing();
        }

        private void OnWhipSwing()
        {
            // StartCoroutine(WhipCooldown())
        }
    }
}
