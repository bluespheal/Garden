using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class Veggie : MonoBehaviour
{
    [SerializeField] public float veggie_power;
    [SerializeField] private float veggie_drag;
    [SerializeField] private float veggie_bounciness;

    [SerializeField] private Vector3 initialPos;

    [SerializeField] private PhysicsMaterial2D pMat;

    private void Start()
    {
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
