using Player.Inventory;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CollectibleCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;

        private void Start()
        {
            textMesh.text = "0";
        }
        
        private void Update()
        {
            textMesh.text = InventoryManager.Instance.collectibleCount.ToString();
        }
    }
}
