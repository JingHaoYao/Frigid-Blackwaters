using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownOfFrostIcePrison : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource shatterAudio;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private PolygonCollider2D collider;
    [SerializeField] private GameObject damagingHitbox;
    private SpriteRenderer playerRenderer;
    private int health = 10;

    void Start()
    {
        playerRenderer = FindObjectOfType<PlayerScript>().GetComponent<SpriteRenderer>();
        StartCoroutine(initialDamage());
    }

    void adjustRendererLayer()
    {
        if (Mathf.Abs(playerRenderer.transform.position.y - transform.position.y) < 1.5f && Mathf.Abs(playerRenderer.transform.position.x - transform.position.x) < 3f)
        {
            if (playerRenderer.transform.position.y < transform.position.y)
            {
                spriteRenderer.sortingOrder = playerRenderer.sortingOrder + 5;
            }
            else
            {
                spriteRenderer.sortingOrder = playerRenderer.sortingOrder - 5;
            }
        }
        else
        {
            spriteRenderer.sortingOrder = (200 - (int)((transform.position.y) * 10));
        }
    }

    void Update()
    {
        adjustRendererLayer();
    }

    IEnumerator initialDamage()
    {
        yield return new WaitForSeconds(2f / 12f);
        damagingHitbox.SetActive(false);
        yield return new WaitForSeconds(5f / 12f + 0.25f);
        StartCoroutine(shatter());
    }

    private IEnumerator shatter()
    {
        collider.enabled = false;
        animator.SetTrigger("Shatter");
        shatterAudio.Play();
        yield return new WaitForSeconds(0.417f);
        Destroy(this.gameObject);
    }
}
