using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveInventoryItem
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private string _description;
    [SerializeField]
    private int _price;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private int _amount;

    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public int Price { get => _price; set => _price = value; }
    public Sprite Sprite { get => _sprite; set => _sprite = value; }

    public int Amount { get => _amount; set => _amount = value; }


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
    private int beans;
    [Header("Upgrades")]
    private int apples;
    public int Beans { get => beans; set => beans = value; }
    public int Apples { get => apples; set => apples = value; }

    [Header("Items")]
    public List<SaveInventoryItem> items = new List<SaveInventoryItem>();

    SaveInventoryItem new_item;
    public SaveInventory(Inventory inventory)
    {
        beans = inventory.Beans;
        apples = inventory.Apples;
        
        foreach(InventoryItem item in inventory._items)
        {
            new_item.Name = item.Name;
            new_item.Description = item.Description;
            new_item.Price = item.Price;
            new_item.Sprite = item.Sprite;
            new_item.Amount = item.Amount;
            items.Add(new_item); 
        } 
    }

    public SaveInventory() { }
}
