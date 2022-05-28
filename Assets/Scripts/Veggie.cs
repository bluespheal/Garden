using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using Unity.Rendering;
using UnityEngine.U2D;


public class Veggie : MonoBehaviour
{
    [Header("VeggieLooks")]
    [SerializeField] private Color veggieColor;
    [SerializeField] private TrailRenderer trail;

    [Header("VeggieStats")]
    [SerializeField] private float veggieWeight;

    [SerializeField] private float veggiePower;
    [SerializeField] private float veggieDrag;
    [SerializeField] private float veggieBounciness;

    [SerializeField] private Vector3 initialPos;

    [SerializeField] private PhysicsMaterial2D pMat;

    public float VeggiePower { get => veggiePower; set => veggiePower = value; }
    public float VeggieWeight { get => veggieWeight; set => veggieWeight = value; }

    private void Start()
    {
        trail.startColor = veggieColor;
        trail.endColor = veggieColor;

        GetComponent<Rigidbody2D>().mass = veggieWeight;
        GetComponentInChildren<TrailRenderer>().material.SetColor("_color", veggieColor);
        
        initialPos = transform.position;
        pMat.bounciness = veggieBounciness;
        pMat.friction = veggieDrag;
    }

    void GetVeggiePower(FsmFloat _power)
    {
        _power.Value = veggiePower;
    }

    private void Update()
    {
        if (transform.position.x > 20 || transform.position.x < -20 || transform.position.y < -9 || transform.position.y > 20)
        {
            transform.position = initialPos;
        }
    }

}
