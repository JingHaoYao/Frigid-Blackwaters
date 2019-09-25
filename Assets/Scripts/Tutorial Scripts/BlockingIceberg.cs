using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingIceberg : MonoBehaviour
{
    public Sprite[] brokenSprite;
    Animator animator;
    SpriteRenderer spriteRenderer;
    int whichSprite = 0;
    bool broken = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 16)
        {
            if (whichSprite < brokenSprite.Length - 1)
            {
                whichSprite++;
                spriteRenderer.sprite = brokenSprite[whichSprite];
                FindObjectOfType<AudioManager>().PlaySound("Iceberg Obstacle Crack");
            }
            else
            {
                if (broken == false)
                {
                    FindObjectOfType<AudioManager>().PlaySound("Iceberg Break");
                    animator.enabled = true;
                    animator.SetTrigger("break");
                    Destroy(this.gameObject, 4f / 12f);
                    broken = true;
                }
            }
        }
    }
}
