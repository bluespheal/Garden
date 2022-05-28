using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _sprite;

    [SerializeField] private int _amount;

    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public int Price { get => _price; set => _price = value; }
    public Sprite Sprite { get => _sprite; set => _sprite = value; }

    public int Amount { get => _amount; set => _amount = value; }

    public InventoryItem(SaveInventoryItem saveInventoryItem)
    {
        _name = saveInventoryItem.Name;
        _description = saveInventoryItem.Description;
        _price = saveInventoryItem.Price;
        _sprite = saveInventoryItem.Sprite;
        _amount = saveInventoryItem.Amount;
    }

}
[Serializable] public class InventoryItemList : List<InventoryItem> { }