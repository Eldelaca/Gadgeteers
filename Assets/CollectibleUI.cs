using TMPro;
using UnityEngine;

public class CollectibleUI : MonoBehaviour
{
    public TextMeshProUGUI collectibleText;
    public void ChangeCollectibleCount(int collectibleCount)
    {
        collectibleText.text = collectibleCount.ToString();
    }
}
