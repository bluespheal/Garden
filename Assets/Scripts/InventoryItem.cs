using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    [SerializeField] private int _value;
    [SerializeField] private int _name;
    [SerializeField] private int _description;
    [SerializeField] private Sprite _sprite;

    public int Value { get => _value; set => _value = value; }
    public int Name { get => _name; set => _name = value; }
    public int Description { get => _description; set => _description = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
