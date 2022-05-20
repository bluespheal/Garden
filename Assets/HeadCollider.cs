using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    public PlayerMovement player;
    public Veggie veggie;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (CalculateForce(collision.relativeVelocity.magnitude) > 18f)
            {
                player.Flinch(false);
            }
        }
    }

    float CalculateForce(float magnitude)
    {
        return ((veggie.veggie_weight) * magnitude);
    }
}
