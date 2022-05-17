using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veggie : MonoBehaviour
{
    [SerializeField] private float veggie_power;
    [SerializeField] private float veggie_drag;
    [SerializeField] private float veggie_bounciness;

    [SerializeField] private PhysicsMaterial2D pMat;

    private void Start()
    {
        pMat.bounciness = veggie_bounciness;
        pMat.friction = veggie_drag;
    }

    public float SendVeggiePower(float power)
    {
        return power;
    }
   
}
