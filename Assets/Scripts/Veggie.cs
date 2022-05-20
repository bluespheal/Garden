using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using Unity.Rendering;

public class Veggie : MonoBehaviour
{
    [Header("VeggieLooks")]
    [SerializeField] public Color veggie_color;
    [SerializeField] public Renderer renderer;
    [SerializeField] private TrailRenderer trail;

    [Header("VeggieStats")]
    [SerializeField] public float veggie_weight;

    [SerializeField] public float veggie_power;
    [SerializeField] private float veggie_drag;
    [SerializeField] private float veggie_bounciness;

    [SerializeField] private Vector3 initialPos;

    [SerializeField] private PhysicsMaterial2D pMat;

    private void Start()
    {
        trail.startColor = veggie_color;
        trail.endColor = veggie_color;

        GetComponent<Rigidbody2D>().mass = veggie_weight;
        renderer.material.SetColor("_color", veggie_color);
 
        initialPos = transform.position;
        pMat.bounciness = veggie_bounciness;
        pMat.friction = veggie_drag;
    }

    void GetVeggiePower(FsmFloat _power)
    {
        _power.Value = veggie_power;
    }

    private void Update()
    {
        if (transform.position.x > 20 || transform.position.x < -20 || transform.position.y < -9 || transform.position.y > 20)
        {
            transform.position = initialPos;
        }
    }

}
