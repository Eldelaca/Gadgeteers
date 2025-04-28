using UnityEngine;

namespace Gadgets.BaseGadgets
{
    public class LightningWhip : MonoBehaviour, IGadget
    {
        [Header("References")]
        [SerializeField] private GadgetStats stats;
        [SerializeField] private GameObject playerHandle;
        [SerializeField] private LayerMask whatIsGrapple;
        [SerializeField] private CharacterController player;

        [Header("Grapple Settings")] 
        [SerializeField] private float maxDistance = 1000f;
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

        private void Update()
        {
            if (stats.gadgetId != GadgetManager.Instance.equippedID) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Grapple pressed");
                StartGrapple();
            }

            /*if (Input.GetKeyUp(KeyCode.E))
            {
                StopGrappling();
            }*/

            if (!_isGrappling) return;
            Debug.Log("Grappling");
            GrapplePull();

        }

        public void UseGadget()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;
            
            if (!_canSwing) return;
            OnWhipSwing();
        }

        private void OnWhipSwing()
        {
            // empty for neow
        }

        private void StartGrapple()
        {
            Vector3 mouseDirection = (Input.mousePosition - _grapplePoint).normalized;
            
            if (!Physics.Raycast(_camera.transform.position, mouseDirection, out var hit, Mathf.Infinity,
                    whatIsGrapple)) return;
            
            _grapplePoint = hit.point;

            if (_lr == null) return;
            _lr.SetPosition(0, playerHandle.transform.position);
            _lr.SetPosition(1, _grapplePoint);

            _isGrappling = true;
        }

        private void GrapplePull()
        {
            Vector3 direction = (_grapplePoint - transform.position).normalized;
            
            player.Move(direction * (grappleSpeed * Time.deltaTime));

            if (Vector3.Distance(transform.position, _grapplePoint) < 1f)
            {
                StopGrappling();
            }
        }

        private void StopGrappling()
        {
            Debug.Log("Stopping Grapple");

            _isGrappling = false;

            if (_lr is null) return;
            _lr.SetPosition(0, Vector3.zero);
            _lr.SetPosition(1, Vector3.zero);
        }
    }
}
