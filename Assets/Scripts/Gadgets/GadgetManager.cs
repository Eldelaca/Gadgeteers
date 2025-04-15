using UnityEngine;

namespace Gadgets
{
    public class GadgetManager : MonoBehaviour
    {
        [Header("References")] [Tooltip("Please make sure the order of this array correlates to the order of gadget IDs in the scriptableObjects class")]
        public GameObject[] baseGadgets;

        [Header("Gadget Debugging")] 
        public bool bootsEquipped;
        public bool flamethrowerEquipped;
        public bool lightningWhipEquipped;
        public bool iceBlasterEquip;

        [SerializeField] private Transform playerHandle;
        
        private int _equippedID;
        private GameObject _equippedGadget;
        
        public void OnEquip(int equipID)
        {
            if (GameObject.FindGameObjectsWithTag("Gadget").Length != 0)
            {
                if (equipID == _equippedID)
                {
                    Debug.Log("You already have that equipped");
                    return;
                }
                Destroy(_equippedGadget);
            }
            
            GameObject selectedGadget = baseGadgets[equipID - 1];
            
            _equippedGadget = Instantiate(selectedGadget, playerHandle.position, playerHandle.rotation);
            _equippedGadget.transform.parent = playerHandle;
            
            _equippedID = equipID;
        }
    }
}
