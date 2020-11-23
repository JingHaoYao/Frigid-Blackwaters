using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroStone : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource jumpAudio;
    [SerializeField] private GameObject obstacleHitbox;
    public GameObject deadPyroStone;
    private bool isAttacking = false;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject flameBurst;

    private void Start()
    {
        shadow.SetActive(false);
    }

    private void Update()
    {
        if (isAttacking == false && stopAttacking == false)
        {
            StartCoroutine(jumpAttack());
        }
    }

    IEnumerator jumpAttack()
    {
        isAttacking = true;
        animator.SetTrigger("TakeOff");
        jumpAudio.Play();
        yield return new WaitForSeconds(3f / 12f);
        shadow.SetActive(true);
        obstacleHitbox.SetActive(false);
        Vector3 spotToJump = pickSpotToJumpTo(angleToShip() * Mathf.Deg2Rad);
        float waitDuration = Vector2.Distance(spotToJump, transform.position) / speed;
        LeanTween.move(this.gameObject, spotToJump, waitDuration).setEaseInOutSine();
        yield return new WaitForSeconds(waitDuration);
        animator.SetTrigger("Land");
        yield return new WaitForSeconds(6 / 12f);
        shadow.SetActive(false);

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45;
            GameObject flameBurstInstant = Instantiate(flameBurst, transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 1.5f, Quaternion.identity);
            flameBurstInstant.GetComponent<PyroStoneFlameBurst>().Initialize(this.gameObject);
        }

        obstacleHitbox.SetActive(true);
        yield return new WaitForSeconds(6 / 12f);

        isAttacking = false;
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
        Instantiate(deadPyroStone, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        damageAudio.Play();
        StartCoroutine(hitFrame());

    }
}
