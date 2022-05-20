using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    float xPos;
    [SerializeField] private bool flipped;
    [SerializeField] private int flippedInt;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
