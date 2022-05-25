using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;

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
    [SerializeField] GameObject head;
    [SerializeField] GameObject lowHead;

    WaitForSeconds slideWFS;

    [Header("Attack")]
    [SerializeField] private bool attacking;
    [SerializeField] public GameObject broom;
    [SerializeField] public Animator broomAnimator;
    [SerializeField] float attackLength;
    [SerializeField] GameObject attackHitbox;
    WaitForSeconds attackWFS;

    [Header("Busy")]
    [SerializeField] private bool busy;
    [SerializeField] private float busyLength;
    WaitForSeconds busyWFS;

    [Header("Flinch")]
    [SerializeField] private bool flinching;
    [SerializeField] private float flinchTime;
    WaitForSeconds flinchWFS;

    [Header("Damage")]
    [SerializeField] private bool hurting;
    [SerializeField] private float hurtTime;
    WaitForSeconds hurtingWFS;

    [Header("Blinking")]
    [SerializeField] private bool blinking;
    [SerializeField] private float blinkTime;
    WaitForSeconds blinkingWFS;

    [Header("Dead")]
    [SerializeField] private bool dying;
    [SerializeField] private float deathTime;
    WaitForSeconds dyingWFS;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 2 + GameManager.Instance.Inventory.GetApples();
        currentHealth = maxHealth;
        busyWFS = new WaitForSeconds(busyLength);
        attackWFS = new WaitForSeconds(attackLength);
        slideWFS = new WaitForSeconds(slideLength);
        flinchWFS = new WaitForSeconds(flinchTime);
        hurtingWFS = new WaitForSeconds(hurtTime);
        blinkingWFS = new WaitForSeconds(blinkTime);
        dyingWFS = new WaitForSeconds(deathTime);
    }

    void Update()
    {
        if (GameManager.Instance.paused)
            return;
        if (attacking || flinching || hurting || dying)
            return;

        if (!sliding || !attacking || !flinching || !hurting || !dying)
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
        if (GameManager.Instance.paused)
            return;
        Slide();
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.paused)
            return;
        Attack();
    }
    void OnPause(InputValue value)
    {
        GameManager.Instance.TogglePause();
    }
    public void Slide()
    {
        if (busy || flinching || hurting || dying) return;
        sliding = true;
        busy = true;
        if (!animator.GetBool("Sliding"))
            animator.SetBool("Sliding", true);
        StartCoroutine(Sliding());
    }

    public void Attack()
    {
        if (busy || flinching || hurting || dying) return;
        attacking = true;
        busy = true;
        animator.SetTrigger("Attack");

        broom.SetActive(true);
        broomAnimator.SetTrigger("Broom");
    
        StartCoroutine(Attacking());
    }
    public void Flinch()
    {
        flinching = true;
        if (!animator.GetBool("Flinch"))
            animator.SetBool("Flinch", true);
        StartCoroutine(Flinching());
    }

    public void Damage()
    {
        if (hurting || dying)
            return;
        hurting = true;
        GameManager.Instance.ForestUIManager.UpdateHeartBar(currentHealth, true);
        if (!animator.GetBool("Flinch"))
            animator.SetBool("Flinch", true);
        if (!animator.GetBool("Blink"))
            animator.SetBool("Blink", true);

        ReceiveDamage();

        if (!dying)
            GameManager.Instance.AudioManager.PlaySFX(2);
            StartCoroutine(Hurting());
    }

    public void Heal()
    {
        currentHealth++;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            GameManager.Instance.ForestUIManager.UpdateHeartBar(currentHealth, false);
        }
    }

    public void Die()
    {
        GameManager.Instance.AudioManager.PlaySFX(3);
        if (dying)
            return;
        dying = true;
        if (!animator.GetBool("Flinch"))
            animator.SetBool("Flinch", true);
        head.SetActive(false);
        lowHead.SetActive(false);
        StartCoroutine(Dying());
    }

    public void ReceiveDamage()
    {
        currentHealth--;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth == 0)
        {
            Die();
        }
    }

    IEnumerator Sliding()
    {
        slideHitbox.SetActive(true);
        head.SetActive(false);
        lowHead.SetActive(true);
        yield return slideWFS;
        sliding = false;
        animator.SetBool("Sliding", false);
        slideHitbox.SetActive(false);
        head.SetActive(true);
        lowHead.SetActive(false);
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
        print("flinching");
        animator.SetBool("Flinch", false);

    }

    IEnumerator Hurting()
    {
        
        hurting = true;
        StartCoroutine(Blinking());
        head.SetActive(false);
        lowHead.SetActive(false);
        yield return hurtingWFS;
        hurting = false;
        print("hurting");
        if (!dying)
            animator.SetBool("Flinch", false);
    }
    IEnumerator Blinking()
    {
        yield return blinkingWFS;
        blinking = false;
        animator.SetBool("Blink", false);
        head.SetActive(true);
        lowHead.SetActive(true);
    }

    IEnumerator Dying()
    {
        dying = true;
        animator.SetBool("Blink", false);
        animator.SetBool("Flinch", true);
        GameManager.Instance.canTogglePause = false;
        yield return dyingWFS;
        GameManager.Instance.SceneChanger.ChangeLevel("MainMenu");
    }

    IEnumerator EndBusy()
    {
        yield return busyWFS;
        busy = false;
    }
}
