using UnityEngine;
using Gadgets; // Gadget Manager
using Gadgets.BaseGadgets; // Sub Class from other scripts

/// <summary>
/// Exposes UI hooks to equip/unequip only the Flamethrower and Ice Blaster gadgets,
/// retrieving the gadgetId directly from each gadget's ScriptableObject stats.
/// Hook OnToggleFlamethrower and OnToggleIceBlaster up to your UI buttons.
/// </summary>
public class WeaponToggle : MonoBehaviour 
{
    [Header("Drag & drop gadget components here (must have a Stats SO assigned)")]
    public Flamethrower flamethrower;
    public IceGun iceBlaster;

    /// <summary>
    /// UI handler: toggles the Flamethrower gadget via its own GadgetStats SO.
    /// </summary>
    public void OnToggleFlamethrower()
    {
        if (flamethrower == null || flamethrower.Flamestats == null) return;
        ToggleGadget(flamethrower, flamethrower.Flamestats.gadgetId);
    }

    /// <summary>
    /// UI handler: toggles the Ice Blaster gadget via its own GadgetStats SO.
    /// </summary>
    public void OnToggleIceBlaster()
    {
        if (iceBlaster == null || iceBlaster.IceGunStats == null) return;
        ToggleGadget(iceBlaster, iceBlaster.IceGunStats.gadgetId);
    }

    /// <summary>
    /// Core toggle logic using IGadget interface and SO data.
    /// If it's already equipped, calls UnEquip(); otherwise Equip().
    /// </summary>
    private void ToggleGadget(IGadget gadget, int gadgetId)
    {
        if (GadgetManager.Instance.equippedID == gadgetId)
        {
            gadget.UnEquip();
        }
        else
        {
            gadget.Equip();
        }
    }
}
