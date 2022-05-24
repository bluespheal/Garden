using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Furniture", menuName = "Furniture")]
public class Furniture : ScriptableObject
{
    public string _name;
    public string _description;
    public string _price;
    public Sprite _sprite;
    public int _class;
}

