using UnityEngine;
using Gadgets; // Gadget Manager
using Gadgets.BaseGadgets; // Sub Class from other scripts
using Gadgets.ComboGadgets; // Sub Class from other scripts

/// <summary>
/// This Code Acts more so for the UI
/// Probably will change when UI is set up
/// this Toggles from the Ui instead of the individual weapons
/// More so like a list
/// </summary>
public class WeaponToggle : MonoBehaviour 
{
    [Header("Drag & drop gadget components here (must have a Stats SO assigned)")]
    public Flamethrower flamethrower;
    public IceGun iceBlaster;
    public FireTornado fireTornadoGadget;

    public void OnToggleFlamethrower()
    {
        if (flamethrower == null || flamethrower.Flamestats == null) return;
        ToggleGadget(flamethrower, flamethrower.Flamestats.gadgetId);
    }

    
    public void OnToggleIceBlaster()
    {
        if (iceBlaster == null || iceBlaster.IceGunStats == null) return;
        ToggleGadget(iceBlaster, iceBlaster.IceGunStats.gadgetId);
    }


    public void OnToggleFireTornado()
    {
        if (fireTornadoGadget == null || fireTornadoGadget.tornadoStats == null) return;
        ToggleGadget(fireTornadoGadget, fireTornadoGadget.tornadoStats.gadgetId);
    }

    /// <summary>
    /// Toggle logic using IGadget interface and SO data.
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
