using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    float xPos;
    [SerializeField] private bool flipped;
    [SerializeField] private int flippedInt;
    public Animator animator;

    [Header("Move")]
    [SerializeField] Vector3 moveVal;
    [SerializeField] float moveSpeed;

    [Header("Slide")]
    [SerializeField] private bool sliding;
    [SerializeField] float slideSpeed;
    [SerializeField] float slideLength;
    [SerializeField] GameObject slideHitbox;

    [Header("Attack")]
    [SerializeField] private bool attacking;
    [SerializeField] public GameObject broom;
    [SerializeField] public Animator broomAnimator;
    [SerializeField] float attackLength;
    [SerializeField] GameObject attackHitbox;
    [Header("Busy")]
    [SerializeField] private bool busy;
    [SerializeField] private float busyLength;

    [Header("Flinch")]
    [SerializeField] private bool flinching;
    [SerializeField] private float flinchTime;

    WaitForSeconds attackWFS;
    WaitForSeconds slideWFS;
    WaitForSeconds busyWFS;

    WaitForSeconds flinchWFS;

    // Start is called before the first frame update
    void Start()
    {
        busyWFS = new WaitForSeconds(busyLength);
        attackWFS = new WaitForSeconds(attackLength);
        slideWFS = new WaitForSeconds(slideLength);
        flinchWFS = new WaitForSeconds(flinchTime);
    }

    void Update()
    {
        if (attacking || flinching)
            return;

        if (!sliding || !attacking || !flinching)
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

    void OnAttack(InputValue value)
    {
        Attack();
    }

    public void Slide()
    {
        if (busy || flinching) return;
        sliding = true;
        busy = true;
        if (!animator.GetBool("Sliding"))
            animator.SetBool("Sliding", true);
        StartCoroutine(Sliding());
    }

    public void Attack()
    {
        if (busy || flinching) return;
        attacking = true;
        busy = true;
        animator.SetTrigger("Attack");

        broom.SetActive(true);
        broomAnimator.SetTrigger("Broom");
    
        StartCoroutine(Attacking());
    }
    public void Flinch(bool damage)
    {
        if (damage){
            ReceiveDamage();
        }
        flinching = true;
        if (!animator.GetBool("Flinch"))
            animator.SetBool("Flinch", true);
        StartCoroutine(Flinching());
    }

    public void ReceiveDamage()
    {
        Debug.Log("Damage! Ouch!");
    }

    IEnumerator Sliding()
    {
        slideHitbox.SetActive(true);
        yield return slideWFS;
        sliding = false;
        animator.SetBool("Sliding", false);
        slideHitbox.SetActive(false);
        StartCoroutine(EndBusy());
    }

    IEnumerator Attacking()
    {
        attackHitbox.SetActive(true);
        yield return attackWFS;
        attacking = false;
        attackHitbox.SetActive(false);
        broom.SetActive(false);
        StartCoroutine(EndBusy());
    }

    IEnumerator Flinching()
    {
        flinching = true;
        yield return flinchWFS;
        flinching = false;
        animator.SetBool("Flinch", false);
    }

    IEnumerator EndBusy()
    {
        yield return busyWFS;
        busy = false;
    }
}
