using UnityEngine;

namespace Gadgets
{
    [CreateAssetMenu(fileName = "GadgetStats", menuName = "Scriptable Objects/GadgetStats")]
    public class GadgetStats : ScriptableObject
    {
        [Header("Gadget ID")]
        [Tooltip("Gadgets Unique ID")]
        public int gadgetId;
        
        [Header("Gadget Damage Values")]
        [Tooltip("How much damage the gadget deals per tick, set to 0 if redundant")]
        public float gadgetDamageTick;
        [Tooltip("Clamp value for gadget damage across multiple instances, set to same value as Gadget Damage Tick if only a single instance of damage occurs")]
        public float gadgetMaxDamage;
        
        [Header("Gadget Damage Over Time Values")]
        [Tooltip("How many times this gadget deals its Gadget Damage Tick Value as damage per use, set to 0 if redundant")]
        public int gadgetDamageInstances; 
        [Tooltip("How many frames pass between each Gadget Damage Instance")]
        public float gadgetDamageInstanceCooldown;
        
        [Header("Gadget Cooldown Values")]
        [Tooltip("How many frames before the gadget can be used again")]
        public float useCooldown;
        [Tooltip("How many frames after initial use before the gadget's cooldown timer begins")]
        public float useDuration; 

        [Header("Boots Exclusive Values")]
        [Tooltip("How many additional jumps do the boots grant you, 1 is a double jump, 2 a triple, etc. Set to 0 if not boots")]
        public int additionalJumpCount; 
        
        [Header("UI Values")]
        [Tooltip("UI Sprite Storage")]
        public Sprite gadgetIcon; 
    }
}
