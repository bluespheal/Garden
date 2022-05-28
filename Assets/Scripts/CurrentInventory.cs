using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory")]
public class CurrentInventory : ScriptableObject {

    [SerializeField]
    Inventory inventory;
    public Inventory Inventory { get => inventory; set => inventory = value; }
    
    public void AddBeans(int number)
    {
        Inventory.Beans += number;
    }

    public void SpendBeans(int number)
    {
        Inventory.Beans -= number;
    }
}
