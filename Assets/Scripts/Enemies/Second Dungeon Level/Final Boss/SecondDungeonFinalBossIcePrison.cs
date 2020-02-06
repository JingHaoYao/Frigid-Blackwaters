using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDungeonFinalBossIcePrison : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource shatterAudio;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private PolygonCollider2D collider;
    [SerializeField] private GameObject damagingHitbox;
    private SpriteRenderer playerRenderer;
    private int health = 10;
    public YlvaCompanion ylvaCompanion;

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
    }

    IEnumerator damageTick()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator shatter(bool fireKill = false)
    {
        collider.enabled = false;
        if (fireKill == false)
        {
            animator.SetTrigger("Shatter");
        }
        else
        {
            animator.SetTrigger("Fire Shatter");
        }
        shatterAudio.Play();
        yield return new WaitForSeconds(0.417f);
        Destroy(this.gameObject);
    }

    public void forceShatter()
    {
        StartCoroutine(shatter(false));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            health -= collision.gameObject.GetComponent<DamageAmount>().damage;
            damageAudio.Play();
            StartCoroutine(damageTick());
            if(health <= 0)
            {
                StartCoroutine(shatter());
            }

            ylvaCompanion.triggerFireball(collision.transform.position);
        }
        else if(collision.gameObject.name == "Ylva's Fireball(Clone)") 
        {
            StartCoroutine(shatter(true));
        }
    }
}
