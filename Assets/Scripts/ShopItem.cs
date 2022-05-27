using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShopItem", menuName = "ShopItem")]
public class ShopItem : ScriptableObject
{
    public string _name;
    public string _description;
    public string _price;
    public Sprite _sprite;
    public int amount;
}

