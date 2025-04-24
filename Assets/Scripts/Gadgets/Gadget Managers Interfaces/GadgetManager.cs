using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Changes where added to the OnEquip() method [Lines 81+]
// In case this gets changed....

namespace Gadgets
{
    public class  GadgetManager : MonoBehaviour
    {
        [Header("References")] [Tooltip("Please make sure the order of this array correlates to the order of gadget IDs in the scriptableObjects class")]
        public GameObject[] baseGadgets;

        [Header("Gadget Debugging")] 
        public bool bootsEquipped;
        public bool flamethrowerEquipped;
        public bool lightningWhipEquipped;
        public bool iceBlasterEquip;

        [SerializeField] private Transform playerHandle;
        
        public int equippedID;
        public GameObject equippedGadget;
        private List<IGadget> _gadgetObjects;
        public static GadgetManager Instance { get; private set; }


        private void Start()
        {
            OnEquip(3); // This will equip the gadget with ID 1 at scene start
        }


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one gadget manager! Destroyed the Imposter");
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _gadgetObjects = FindAllGadgetObjects();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            // Empty
        }

        public void OnEquip(int equipID)
        {
            if (GameObject.FindGameObjectsWithTag("Gadget").Length != 0)
            {
                Debug.Log("You already have an item equipped");
                return;
            }
            
            GameObject selectedGadget = baseGadgets[equipID - 1];
            
            equippedGadget = Instantiate(selectedGadget, playerHandle.position, playerHandle.rotation);
            equippedGadget.transform.parent = playerHandle;


            // This just allows the script to just check if the gadget has been equipped for testing
            // Grabs the Equip from that object and equips it
            // Checking if the weapon is or not equipped
            /*
            IGadget gadgetScript = equippedGadget.GetComponent<IGadget>(); // Get the gadget script

            if (gadgetScript != null)
            {
                gadgetScript.Equip(); // Call Equip() for this gadget
            }
            else
            {
                Debug.LogError("The gadget does not have the IGadget Component");
            }
            */

            equippedID = equipID;
        }
        
        public void OnUnEquip()
        {
            if (GameObject.FindGameObjectsWithTag("Gadget").Length == 0)
            {
                Debug.Log("You have no item eqipped");
                return;
            }
            
            foreach (IGadget gadgetObject in _gadgetObjects)
            {
                gadgetObject.UnEquip();
            }
            
            Destroy(equippedGadget);
            equippedID = 0;
            
        }

        private List<IGadget> FindAllGadgetObjects()
        {
            IEnumerable<IGadget> gadgetObjects =
                FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID).OfType<IGadget>();
            
            return new List<IGadget>(gadgetObjects);
        }
    }
}
