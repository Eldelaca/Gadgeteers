using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gadgets.BaseGadgets
{
    public class LightningWhip : MonoBehaviour, IGadget
    {
        [Header("References")]
        [SerializeField] private GadgetStats stats;
        [SerializeField] private LayerMask grappleLayer;
        [SerializeField] private CharacterController cc;
        [SerializeField] private GameObject lightningEffect;

        [Header("Grapple Settings")]
        [SerializeField] private float grappleRange = 25f;
        [SerializeField] private float grappleSpeed = 50f;
        
        [SerializeField] private bool enableGrapple = true;

        private GameObject player;  
        private bool _canSwing;
        private bool _isGrappling;
        private Vector3 _grapplePoint;
        

        private BoxCollider _collider;
        private Transform cameraTransform;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            cc = player.GetComponent<CharacterController>();

            if (player == null)
            {
                Debug.LogError("Player GameObject with tag 'Player' not found in the scene!");
            }

            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("MainCamera not found in the scene!");
            }
        }

        private void OnEnable()
        {
            _collider = GetComponent<BoxCollider>();
        }

        public void Equip()
        {
            if (stats.gadgetId != GadgetManager.Instance.equippedID) return;

            _canSwing = true;
            GadgetManager.Instance.lightningWhipEquipped = true; // Debugging only pwease remove after :3
        }

        public void UnEquip()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            _canSwing = false;
            GadgetManager.Instance.bootsEquipped = false; // Debugging only pwease remove after :3
        }

        public void UseGadget()
        {
            if (GadgetManager.Instance.equippedID != stats.gadgetId) return;

            if (!_canSwing) return;
            Debug.Log("Using Lightning Whip");

            OnWhipSwing();
            if (enableGrapple)
                TryGrapple();
        }

        private void OnWhipSwing()
        {
            lightningEffect.SetActive(true);
            StartCoroutine(WhipDamageOverTime());
            _canSwing = false;
            StartCoroutine(WhipCooldown());
        }

        private IEnumerator WhipDamageOverTime()
        {
            _collider.enabled = true;
            yield return new WaitForSeconds(stats.useDuration);
            lightningEffect.SetActive(false);
            _collider.enabled = false;
        }

        private IEnumerator WhipCooldown()
        {
            yield return new WaitForSeconds(stats.useCooldown);
            _canSwing = true;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                TryGrapple();

            }

            if (_isGrappling)
            {

                PerformGrapple();
            }

            if (Input.GetKeyDown(KeyCode.E) && _isGrappling)
            {
                StopGrappling();
            }

        }

        private void TryGrapple()
        {
            if (cameraTransform == null || player == null)
            {
                Debug.LogError("Camera Transform or Player not set!");
                return;
            }

            // Takes the visual Mouse Cursor and use its inputs
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            // Detects click Input if in range
            if (Physics.Raycast(ray, out RaycastHit hit, grappleRange, grappleLayer))
            {
                cc.enabled = false;
                _grapplePoint = hit.point;
                _isGrappling = true;
                Debug.Log("Grapple Point found at: " + hit.point);
            }
            else
            {
                Debug.Log("No valid grapple point found.");
            }
        }

        private void PerformGrapple()
        {
            
            Vector3 direction = (_grapplePoint - player.transform.position).normalized;
            float distance = Vector3.Distance(player.transform.position, _grapplePoint);

            // If the player is close enough to the grapple point
            if (distance < 2f)
            {
                Debug.Log("Reached the grapple point.");
                cc.enabled = false;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StopGrappling();
                }

                return;
            }

            // Move the player towards the grapple point
            player.transform.position = Vector3.MoveTowards(player.transform.position, _grapplePoint, grappleSpeed * Time.deltaTime);
        }

        private void StopGrappling()
        {
            cc.enabled = true;
            _isGrappling = false; // Stop grappling


        }

    }
}
