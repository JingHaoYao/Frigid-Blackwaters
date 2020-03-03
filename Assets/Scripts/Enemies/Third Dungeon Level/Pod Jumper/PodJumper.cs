using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodJumper : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource jumpAudio;
    [SerializeField] private AudioSource landingAudio;
    [SerializeField] Collider2D takeDamageHitbox;
    [SerializeField] Collider2D landingAttackHitbox;
    [SerializeField] private GameObject obstacleHitbox;
    public GameObject deadPod;
    private bool isAttacking = false;
    private bool bloomed = false;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject shadow;
    private float attackWaitPeriod = 0;

    private void Start()
    {
        landingAttackHitbox.enabled = false;
        shadow.SetActive(false);
    }

    private void Update()
    {
        if (attackWaitPeriod < (bloomed ? 0.5f : 1))
        {
            attackWaitPeriod += Time.deltaTime;
        }
        else
        {
            if (isAttacking == false && stopAttacking == false)
            {
                StartCoroutine(jumpAttack());
            }
        }
    }

    IEnumerator jumpAttack()
    {
        isAttacking = true;
        animator.SetTrigger("WindUp");
        yield return new WaitForSeconds(4f / 12f);
        shadow.SetActive(true);
        jumpAudio.Play();
        obstacleHitbox.SetActive(false);
        takeDamageHitbox.enabled = false;
        Vector3 spotToJump = pickSpotToJumpTo(angleToShip() * Mathf.Deg2Rad);
        float waitDuration = Vector2.Distance(spotToJump, transform.position) / speed;
        LeanTween.move(this.gameObject, spotToJump, waitDuration).setEaseInOutSine();
        yield return new WaitForSeconds(waitDuration);
        animator.SetTrigger("WindDown");
        yield return new WaitForSeconds(3 / 12f);
        shadow.SetActive(false);
        landingAudio.Play();
        obstacleHitbox.SetActive(true);
        takeDamageHitbox.enabled = true;
        landingAttackHitbox.enabled = true;
        yield return new WaitForSeconds(1 / 12f);
        landingAttackHitbox.enabled = false;
        yield return new WaitForSeconds(5/12f);
        isAttacking = false;
        attackWaitPeriod = 0;
        animator.SetTrigger("Idle");
    }

    Vector3 pickSpotToJumpTo(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector3 returningPosition = transform.position + directionVector * Mathf.Clamp(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition), 0, 6.5f);
        while (Physics2D.OverlapCircle(returningPosition, 0.4f, layerMask) && Vector2.Distance(returningPosition, transform.position) > 0.5f)
        {
            returningPosition -= directionVector * 0.5f;
        }
        return returningPosition;
    }

    private float angleToShip()
    {
        return (Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Bloom Status Effect" || newStatus.name == "Bloom Status Effect (Clone)")
        {
            bloomed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public override void deathProcedure()
    {
        Instantiate(deadPod, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        damageAudio.Play();
        StartCoroutine(hitFrame());
    }
}
