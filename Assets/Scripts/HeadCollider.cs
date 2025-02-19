using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement player;
    [SerializeField]
    private Veggie veggie;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (CalculateForce(collision.relativeVelocity.magnitude) > 18f)
            {
                GameManager.Instance.AudioManager.PlaySFX(5);
                player.Flinch();
            }
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            player.Damage();
        }
    }

    float CalculateForce(float magnitude)
    {
        return ((veggie.VeggieWeight) * magnitude);
    }
}
