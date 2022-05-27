using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory
{
    [Header("Money")]
    public int beans;
    [Header("Upgrades")]
    public int apples;

    [Header("Items")]
    public List<InventoryItem> _items = new List<InventoryItem>();

    //[Header("Veggies")]
    //public List<Veggie> veggies_keys = new List<Veggie>();
    //public List<int> veggies_values = new List<int>();
    //public Dictionary<Veggie, int> veggies = new Dictionary<Veggie, int>();
    //[Header("Seeds")]
    //public List<Seed> seeds_keys = new List<Seed>();
    //public List<int> seeds_values = new List<int>();
    //public Dictionary<Seed, int> seeds = new Dictionary<Seed, int>();
    //[Header("Furniture")]
    //public List<Furniture> furniture_keys = new List<Furniture>();
    //public List<int> furniture_values = new List<int>();
    //
    //private Dictionary<Furniture, int> _furniture = new Dictionary<Furniture, int>();
    //public Dictionary<Furniture, int> FurnitureList => _furniture;
    //public int GetItemsCount => FurnitureList.Count;

    public Inventory(int beans, int apples, List<InventoryItem> items)
    {
        this.beans = beans;
        this.apples = apples;
        this._items = items;
    }

    public Inventory() { }
    
}
