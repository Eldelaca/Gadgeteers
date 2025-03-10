using UnityEngine;

public class CombinationInterface : MonoBehaviour
{
    // References for UI frames
    [SerializeField] private GameObject combinationFrame,
        openFrame;
/*iceGunPrefab,
bootsPrefab; */


    public Sprite openFrameSprite;

    public bool iceGunEquip,
        bootsEquip,
        iceBootsEquip;

    private bool _firstSelected;
    private bool _secondSelected;

    private string _firstSelection;
    private string _secondSelection;


    public void Combine()
    {
        if (_firstSelection == "IceGun" && _secondSelection == "Boots")
        {
            iceBootsEquip = true;
        }
    }

    public void IceGunSelect()
    {
        if (!_firstSelected)
        {
            _firstSelected = true;
            _firstSelection = "IceGun";
        }
        else if (_firstSelected && !_secondSelected)
        {
            _secondSelected = true;
            _secondSelection = "IceGun";
        }
        else
        {
            Debug.Log("Combination interface overload");
        }
    }

    public void BootsSelect()
    {
        if (!_firstSelected)
        {
            _firstSelected = true;
            _firstSelection = "Boots";
        }
        else if (_firstSelected && !_secondSelected)
        {
            _secondSelected = true;
            _secondSelection = "Boots";
        }
        else
        {
            Debug.Log("Combination interface overload");
        }
    }
        
        
    // opening and closing the window
    public void OpenWindow()
    {
        combinationFrame.SetActive(true);
        openFrame.SetActive(false);
    }
    
    public void CloseWindow()
    {
        combinationFrame.SetActive(false);
        openFrame.SetActive(true);
    }
}
