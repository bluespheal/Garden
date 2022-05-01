using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCollider : MonoBehaviour
{
    [SerializeField] private float slideForce;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            Debug.DrawRay(col.contacts[0].point, col.contacts[0].normal* -10, Color.green, 1, false);
            col.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(slideForce * col.contacts[0].normal* - 1);
        }
    }

}
