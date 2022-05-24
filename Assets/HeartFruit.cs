using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HeartFruit : MonoBehaviour
{
    public VisualEffect collectVfx;
    public PlayerMovement player;
    private bool collected;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Millet").GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        Invoke("StartFading", 7.0f);
    }

    private void StartFading()
    {
        anim.Play("Blink");
        Invoke("DestroyMe", 4.0f);

    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Collect());
            player.Heal();
        }
    }

    public IEnumerator Collect()
    {
        CancelInvoke();
        collectVfx.Play();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;        
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
