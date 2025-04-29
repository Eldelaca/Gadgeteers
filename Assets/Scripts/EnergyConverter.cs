using System;
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
    
    private static readonly int Open = Animator.StringToHash("DoorOpen");
    

    private bool _animationPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "LightningWhip(Clone)") ChargePulse();
    }

    private void ChargePulse()
    {
        IsPowered = !IsPowered;
        powerCircle.SetActive(IsPowered);

        if (IsPowered)
        {
            DoorOpen();
        }
    }

    private void DoorOpen()
    {
        doorAnimator.SetTrigger(Open);
    }
}
