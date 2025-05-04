using System;
using System.Collections;
using UnityEngine;

public class EnergyConverter : MonoBehaviour
{
    public bool IsPowered { get; private set; }
    
    [Header("References")]
    [SerializeField] private GameObject powerCircle;
    
    [Header("References")]
    [SerializeField] private GameObject transformer;
    [SerializeField] private Animator doorAnimator;
    
    private EnergyConverter _energyConverter;

    private bool _powerCooldown;
    
    private static readonly int Open = Animator.StringToHash("DoorOpen");
    

    private bool _animationPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name is "LightningWhip(Clone)" or 
            "TornadoPrefab(Clone)" or 
            "SpeedBoots(Clone)" or 
            "StormPrefab(Clone)") ChargePulse();
    }

    private void ChargePulse()
    {
        if (_powerCooldown) return;
        
        IsPowered = !IsPowered;
        powerCircle.SetActive(IsPowered);

        if (IsPowered)
        {
            DoorOpen();
        }

        _powerCooldown = true;

        StartCoroutine(ChargeCooldown());
    }


    private IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(1f);
        _powerCooldown = false;
    }
    
    private void DoorOpen()
    {
        doorAnimator.SetTrigger(Open);
    }
}
