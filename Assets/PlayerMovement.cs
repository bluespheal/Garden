using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;

    float xPos;
    [SerializeField] private bool flipped;
    [SerializeField] private int flippedInt;
    public Animator animator;

    [SerializeField] Vector3 moveVal;
    [SerializeField] float moveSpeed;

    [SerializeField] private bool sliding;
    [SerializeField] float slideSpeed;
    [SerializeField] float slideLength;
    [SerializeField] GameObject slideHitbox;

    [SerializeField] private bool attacking;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if(!sliding)
        {
            transform.Translate(new Vector3(moveVal.x, 0, 0) * moveSpeed * Time.deltaTime);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveVal.x));

        if (sliding)
        {
            transform.Translate(new Vector3(flippedInt, 0, 0) * slideSpeed * Time.deltaTime);
        }

        SetLimit();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        
        moveVal = value.Get<Vector2>().normalized;

        if (moveVal.x < 0.9 && moveVal.x > -0.9)
            moveVal.x = 0; 
    }

    void SetLimit()
    {
        xPos = Mathf.Clamp(transform.position.x, leftLimit.position.x, rightLimit.position.x);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
    void FlipSprite()
    {
        if (!sliding)
        {
            if (moveVal.x > 0)//Mathf.Round(moveVal.x) == 1)
            {
                flippedInt = 1;
                flipped = false;
            }
            else if (moveVal.x < 0)//Mathf.Round(moveVal.x) == -1)
            {
                flippedInt = -1;
                flipped = true;
            }
        }
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
        if (!animator.GetBool("Sliding"))
            animator.SetBool("Sliding", true);
        StartCoroutine(Sliding());
    }

    IEnumerator Sliding()
    {
        slideHitbox.SetActive(true);
        yield return new WaitForSeconds(slideLength);
        sliding = false;
        animator.SetBool("Sliding", false);
        slideHitbox.SetActive(false);
    }


}
