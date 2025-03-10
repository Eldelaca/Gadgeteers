using System;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] 
        private int itemId;
        private string itemName;
        private GameObject inventory;
        private GameObject slot;

        private void Awake()
        {
            inventory = GameObject.Find("InventoryBar");
        }

        private void OnWeaponSelect()
        {
            
        }
}
