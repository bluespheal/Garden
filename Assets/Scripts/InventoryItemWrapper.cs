using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveInventoryItem
{
    public string _name;
    public string _description;
    public int _price;
    public Sprite _sprite;
    public int _amount;
    public SaveInventoryItem(InventoryItem _item)
    {
        _name = _item.Name;
        _description = _item.Description;
        _price = _item.Price;
        _sprite = _item.Sprite;
        _amount = _item.Amount;
    }
}

[System.Serializable]

public class SaveInventory
{
    [Header("Money")]
    public int beans;
    [Header("Upgrades")]
    public int apples;

    [Header("Items")]
    public List<SaveInventoryItem> items = new List<SaveInventoryItem>();

    SaveInventoryItem new_item;
    public SaveInventory(Inventory inventory)
    {
        beans = inventory.beans;
        apples = inventory.apples;
        
        foreach(InventoryItem item in inventory._items)
        {
            new_item._name = item.Name;
            new_item._description = item.Description;
            new_item._price = item.Price;
            new_item._sprite = item.Sprite;
            items.Add(new_item); 
        } 
    }

    public SaveInventory() { }
}
