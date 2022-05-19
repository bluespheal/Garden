using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class bean : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] List<int> values;

    [Header("Sprites")]
    [SerializeField] List<Sprite> sprites;

    [Header("Chances")]
    public List<float> chances;

    public float random;
    public VisualEffect collectVfx;

    private bool collected;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("StartFading", 6.0f);
        random = Random.Range(1, (chances[0] + chances[1] + chances[2] + chances[3] + chances[4]));

        if (random < chances[0])
        {
            GenerateBean(0);
        }
        else if (random < chances[0] + chances[1])
        {
            GenerateBean(1);
        }
        else if (random < chances[0] + chances[1] + chances[2])
        {
            GenerateBean(2);
        }
        else if (random < chances[0] + chances[1] + chances[2] + chances[3])
        {
            GenerateBean(3);
        }
        else if (random < chances[0] + chances[1] + chances[2] + chances[3] + chances[4])
        {
            GenerateBean(4);
        }
        else
        {
            GenerateBean(0);
            Debug.LogError("Something is wrong with the powerup random seed");
        }

    }

    private void StartFading()
    {
        anim.Play("Blink");
        Invoke("DestroyMe", 3.0f);

    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
    private void GenerateBean(int num)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[num];
        transform.localScale = transform.localScale * (0.5f+(num*0.33f)); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Collect());
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
