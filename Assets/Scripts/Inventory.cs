using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] private int beans;

    [Header("Veggies")]
    [SerializeField] private Veggie[] veggies;
    [Header("Seeds")]
    [SerializeField] private Seed[] seeds;
    [Header("Furniture")]
    [SerializeField] private Furniture[] furniture;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void AddBeans(int number)
    {
        beans += number;
    }

    public void SpendBeans(int number)
    {
        beans -= number;
    }
}
