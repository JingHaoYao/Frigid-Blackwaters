using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlessSpearman : MonoBehaviour
{
    public Sprite facingLeft, facingUp, facingDown, facingRight;
    public GameObject leftFacingHitbox, downFacingHitbox, upFacingHitbox, rightFacingHitbox;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public GameObject deadSpearman;
    public float relativeScale = 0.45f;
    GameObject playerShip;
    float stagnantDuration = 0;
    bool poking = false;

    IEnumerator poke()
    {
        animator.enabled = true;
        this.GetComponents<AudioSource>()[0].Play();
        if (spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Attack1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Attack2");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Attack4");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Attack3");
        }
        yield return new WaitForSeconds(4f / 12f);
        if (spriteRenderer.sprite == facingLeft)
        {
            leftFacingHitbox.SetActive(true);
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            downFacingHitbox.SetActive(true);
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            upFacingHitbox.SetActive(true);
        }
        else if (spriteRenderer.sprite = facingRight)
        {
            rightFacingHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(1f / 12f);
        leftFacingHitbox.SetActive(false);
        downFacingHitbox.SetActive(false);
        upFacingHitbox.SetActive(false);
        rightFacingHitbox.SetActive(false);
        animator.enabled = false;
        Instantiate(deadSpearman, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void pickSprite(float direction)
    {
        if (direction > 75 && direction < 105)
        {
            spriteRenderer.sprite = facingUp;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
        else if (direction < 285 && direction > 265)
        {
            spriteRenderer.sprite = facingDown;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
        else if (direction >= 285 && direction <= 360)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(-relativeScale, relativeScale, 0);
        }
        else if (direction >= 180 && direction <= 265)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
        else if (direction < 180 && direction >= 105)
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(-relativeScale, relativeScale, 0);
        }
        else
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = FindObjectOfType<PlayerScript>().gameObject;
        animator.enabled = false;
    }

    void Update()
    {
        float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        if (poking == false)
        {
            pickSprite(angleToShip);
        }
        stagnantDuration += Time.deltaTime;
        if(Vector2.Distance(playerShip.transform.position, transform.position) < 1.7f && poking == false)
        {
            StartCoroutine(poke());
            poking = true;
        }

        if(stagnantDuration >= 4)
        {
            Instantiate(deadSpearman, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
