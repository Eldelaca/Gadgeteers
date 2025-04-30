using System;
using System.Collections.Generic;
using System.Linq;
using Gadgets;
using UnityEngine;
using Player.PlayerCharacterController;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GadgetManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject gadgetManagerUI;
        
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private PlayerLocomotionInput playerLocomotionInput;
        [SerializeField] private PlayerAnimation playerAnimation;
        

        
        private string _passedUI;
        private List<IBoxControls> _boxControls = new List<IBoxControls>();
        private List<IGadget> _gadgets = new List<IGadget>();

        public bool gadgetChanged;
        
        public static GadgetManagerUI Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more than one instance of the GadgetManagerUI");
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _boxControls = FindAllBoxObjects();
            _gadgets = FindAllGadgetObjects();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            // Empty
        }
        
        public void OpenUI(string boxUid)
        {
            _passedUI = boxUid;
            
            playerLocomotionInput.enabled = false;
            playerMovement.enabled = false;
            playerMovement.enabled = false;
            characterController.enabled = false;
            
            gadgetManagerUI.SetActive(true);
        }

        public void CloseUI()
        {
            foreach (var boxObjects in _boxControls)
            {
                boxObjects.ClearBox(_passedUI, gadgetChanged);
            }
            
            playerLocomotionInput.enabled = true;
            playerAnimation.enabled = true;
            characterController.enabled = true;
            playerMovement.enabled = true;
            
            gadgetManagerUI.SetActive(false);

            gadgetChanged = false;
        }
        
        private List<IBoxControls> FindAllBoxObjects()
        {
            IEnumerable<IBoxControls> boxObjects =
                FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID).OfType<IBoxControls>();
            
            return new List<IBoxControls>(boxObjects);
        }
        
        private List<IGadget> FindAllGadgetObjects()
        {
            IEnumerable<IGadget> gadgetObjects =
                FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID).OfType<IGadget>();
            
            return new List<IGadget>(gadgetObjects);
        }
    }
}
