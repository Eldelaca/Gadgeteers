using Gadgets;
using System.Collections.Generic;
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
            List<ComboGadgetStats> checkedCombos = new List<ComboGadgetStats>();

            foreach (ComboGadgetStats comboGadget in comboGadgets)
            {
                if (comboGadget.combinationItem1.gadgetId == firstPassedID ||
                    comboGadget.combinationItem1.gadgetId == secondPassedID)
                {
                    checkedCombos.Add(comboGadget);
                }
            }

            foreach (ComboGadgetStats comboGadgeti in checkedCombos)
            {
                if (comboGadgeti.combinationItem2.gadgetId == secondPassedID ||
                    comboGadgeti.combinationItem2.gadgetId == firstPassedID)
                {
                    int comboGadgetID = comboGadgeti.gadgetId;
                    string gadgetName = comboGadgeti.gadgetName;
                    string gadgetDescription = comboGadgeti.gadgetDescription;
                    Sprite comboGadgetSprite = comboGadgeti.gadgetIcon;

                    gadgetCrafter.FillComboGadgetSlot(comboGadgetID, gadgetName, gadgetDescription, comboGadgetSprite, comboGadgeti.combinationItem1, comboGadgeti.combinationItem2);
                    Debug.Log(comboGadgeti.combinationItem1 + " : " + comboGadgeti.combinationItem2);
                    return;
                }

            }
        }
    }
}
