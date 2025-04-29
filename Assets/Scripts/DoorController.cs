using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject transformer;
    [SerializeField] private Animator doorAnimator;
    
    private EnergyConverter _energyConverter;
    
    private static readonly int Open = Animator.StringToHash("Open");
    

    private bool _animationPlayed;


    private void Start()
    {
        _energyConverter = transformer.GetComponent<EnergyConverter>();
    }

    private void LateUpdate()
    {
        if (!_energyConverter.IsPowered || _animationPlayed) return;
        
        doorAnimator.SetTrigger(Open);
        
        _animationPlayed = true;
        
        
    }
}
