using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory
{
    [Header("Money")]
    [SerializeField] public int beans;
    [Header("Upgrades")]
    [SerializeField] public int apples;
    [Header("Veggies")]
    [SerializeField] public List<Veggie> veggies_keys = new List<Veggie>();
    [SerializeField] public List<int> veggies_values = new List<int>();
    [SerializeField] public Dictionary<Veggie, int> veggies = new Dictionary<Veggie, int>();
    [Header("Seeds")]
    [SerializeField] public List<Seed> seeds_keys = new List<Seed>();
    [SerializeField] public List<int> seeds_values = new List<int>();
    [SerializeField] public Dictionary<Seed, int> seeds = new Dictionary<Seed, int>();
    [Header("Furniture")]
    [SerializeField] public List<Furniture> furniture_keys = new List<Furniture>();
    [SerializeField] public List<int> furniture_values = new List<int>();
    [SerializeField] 
    private Dictionary<Furniture, int> _furniture = new Dictionary<Furniture, int>();
    public Dictionary<Furniture, int> FurnitureList => _furniture;
    public int GetItemsCount => FurnitureList.Count;

    public Inventory(int beans, int apples, Dictionary<Veggie, int> veggies, Dictionary<Seed, int> seeds)
    {
        this.beans = beans;
        this.apples = apples;
        this.veggies = veggies;
        this.seeds = seeds;
    }

    public Inventory() { }
    
}
