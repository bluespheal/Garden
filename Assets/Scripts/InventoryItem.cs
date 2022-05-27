using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class InventoryItem : ScriptableObject
{
    private string _name;
    private string _description;
    private int _price;
    private Sprite _sprite;

    private int _amount;

    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public int Price { get => _price; set => _price = value; }
    public Sprite Sprite { get => _sprite; set => _sprite = value; }

    public int Amount { get => _amount; set => _amount = value; }

    public InventoryItem() { }
}
