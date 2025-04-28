using Gadgets;
using UnityEngine;

namespace UI
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private ComboGadgetStats[] comboGadgets;
        [SerializeField] private GadgetCrafter gadgetCrafter;
        
        public static CraftingManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple CraftingManager in scene.");
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void CraftGadget(int firstPassedID, int secondPassedID)
        {
            foreach (ComboGadgetStats comboGadget in comboGadgets)
            {
                if (comboGadget.combinationItem1.gadgetId == firstPassedID ||
                    comboGadget.combinationItem1.gadgetId == secondPassedID)
                {
                    foreach (ComboGadgetStats comboGadgeti in comboGadgets)
                    {
                        if (comboGadgeti.combinationItem2.gadgetId != secondPassedID ||
                            comboGadgeti.combinationItem2.gadgetId != firstPassedID)
                        {
                            int comboGadgetID = comboGadgeti.gadgetId;
                            string gadgetName = comboGadgeti.gadgetName;
                            string gadgetDescription = comboGadgeti.gadgetDescription;
                            Sprite comboGadgetSprite = comboGadgeti.gadgetIcon;
                            
                            gadgetCrafter.FillComboGadgetSlot(comboGadgetID, gadgetName, gadgetDescription, comboGadgetSprite, comboGadget.combinationItem1, comboGadget.combinationItem2);
                            return;
                        }
                    }
                }
            }
        }
    }
}
