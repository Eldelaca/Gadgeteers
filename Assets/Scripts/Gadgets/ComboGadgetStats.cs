using UnityEngine;

namespace Gadgets
{
    [CreateAssetMenu(fileName = "ComboGadgetStats", menuName = "Scriptable Objects/ComboGadgetStats")]
    public class ComboGadgetStats : GadgetStats
    {
        [Header("Combination Inputs")] 
        [Tooltip("Inputs for Gadget Combination")] public GadgetStats combinationItem1;
        [Tooltip("Inputs for Gadget Combination")] public GadgetStats combinationItem2;
    }
}

