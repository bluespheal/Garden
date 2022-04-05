using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Dreamteck.Splines;

public class FootController : MonoBehaviour
{
    [SerializeField] Vector3 moveValLeft;
    [SerializeField] Vector3 moveValRight;

    [SerializeField] SplinePositioner leftFoot;
    [SerializeField] SplinePositioner rightFoot;

    void Update()
    {
        leftFoot.SetPercent(DigestInput(-moveValLeft.y));
        rightFoot.SetPercent(DigestInput(moveValRight.y));
    }
    void OnLeft(InputValue value)
    {
        moveValLeft = value.Get<Vector2>().normalized;
    }
    void OnRight(InputValue value)
    {
        moveValRight = value.Get<Vector2>().normalized;
    }

    float DigestInput(float _input)
    {
        return Mathf.InverseLerp(-1, 1, _input);
    }
}
