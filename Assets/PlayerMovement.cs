using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool flipped;
    [SerializeField] private bool sliding;
    [SerializeField] private bool attacking;

    [SerializeField] private Rigidbody2D rb2D;

    [SerializeField] float slideSpeed;
    [SerializeField] float slideLength;
    [SerializeField] Vector3 moveVal;
    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
      spriteRenderer = GetComponent<SpriteRenderer>();
      rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!sliding)
            transform.Translate(new Vector3(moveVal.x, 0, 0) * moveSpeed * Time.deltaTime);
        //transform.position += new Vector3( (moveSpeed * Time.deltaTime * moveVal.x), 0, 0);
    }

    void OnMove(InputValue value)
    {
         moveVal = value.Get<Vector2>().normalized;

        if (!sliding)
        {
            if (Mathf.Round(moveVal.x) == 1)
            {
                flipped = false;
            }
            else if (Mathf.Round(moveVal.x) == -1)
            {
                flipped = true;
            }
        }
        
       
        FlipSprite();
    }

    void FlipSprite()
    {
        spriteRenderer.flipX = flipped;
    }

    void OnSlide(InputValue value)
    {
        Slide();
    }

    public void Slide()
    {
        if (sliding) return;
        sliding = true;
        StartCoroutine (Sliding());
    }

    IEnumerator Sliding()
    {
        if (flipped)
        {
            rb2D.AddForce(-transform.right * slideSpeed);
        }
        else
        {
            rb2D.AddForce(transform.right * slideSpeed);
        }
        yield return new WaitForSeconds(slideLength);
        
        rb2D.velocity = Vector3.zero;
        rb2D.angularVelocity = 0;

        sliding = false;
    }

}
