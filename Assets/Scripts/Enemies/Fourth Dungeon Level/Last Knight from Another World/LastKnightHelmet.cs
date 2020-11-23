using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastKnightHelmet : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    LastKnightFromAnotherWorld bossScript;
    [SerializeField] GameObject shadow;
    [SerializeField] AudioSource takeDamageAudio, fireLaserAudio, deathAudio, awakenAudio;
    [SerializeField] Sprite emptyHelmet;
    [SerializeField] GameObject beam;
    [SerializeField] Collider2D takeDamageCollider;

    Coroutine beamAttackRoutine;
    Coroutine attackLoopRoutine;

    private bool isAttacking = false;

    Vector3 positionOnGround = Vector3.zero;

    float angleTravel = 0;

    Vector3 centerPosition = new Vector3(1200, 20, 0);

    float attackPeriod = 0;

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }


    public void Initialize(LastKnightFromAnotherWorld boss)
    {
        this.gameObject.SetActive(true);
        bossScript = boss;
        LeanTween.move(this.gameObject, transform.position + new Vector3(0, 2.5f), 1f);
        LeanTween.value(0, 2.5f, 1f).setEaseOutQuad().setOnUpdate((float val) => { positionOnGround = new Vector3(0, val, 0); }).setOnComplete(() => { attackLoopRoutine = StartCoroutine(attackLoop()); });
        StartCoroutine(positionLoop());
        angleTravel = angleToShip;
    }

    IEnumerator beamAttack()
    {
        isAttacking = true;

        float offSet = 0;

        for (int k = 0; k < 3; k++)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(6 / 12f);


            fireLaserAudio.Play();

            for (int i = 0; i < 4; i++)
            {
                GameObject beamInstant = Instantiate(beam, transform.position, Quaternion.identity);
                beamInstant.GetComponent<LastKnightBeam>().Initialize(this.gameObject, i * 90 + offSet);
            }

            offSet += 45;

            yield return new WaitForSeconds(5 / 12f);
        }

        animator.SetTrigger("Idle");

        isAttacking = false;
    }

    IEnumerator attackLoop()
    {
        while (true)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * speed * Time.deltaTime;

            if(Mathf.Abs(transform.position.x - centerPosition.x) >= 5.5f || Mathf.Abs((transform.position - positionOnGround).y - centerPosition.y) >= 5.5f)
            {
                angleTravel = angleToShip;
            }

            if (isAttacking == false)
            {
                if (attackPeriod < 1)
                {
                    attackPeriod += Time.deltaTime;
                }
                else
                {
                    beamAttackRoutine = StartCoroutine(beamAttack());
                    attackPeriod = 0;
                }
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if (health > 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
        }
    }

    public void reAwaken()
    {
        animator.enabled = true;
        isAttacking = false;
        awakenAudio.Play();
        animator.SetTrigger("Awaken");
        takeDamageCollider.enabled = true;
        LeanTween.move(this.gameObject, transform.position + new Vector3(0, 2.5f), 1f);
        LeanTween.value(0, 2.5f, 1f).setOnUpdate((float val) => { positionOnGround = new Vector3(0, val, 0); }).setOnComplete(() => { attackLoopRoutine = StartCoroutine(attackLoop()); });
        health = maxHealth / 2;
    }

    IEnumerator positionLoop()
    {
        while (true)
        {
            shadow.transform.position = transform.position - positionOnGround;
            spriteRenderer.sortingOrder = (200 - (int)((transform.position.y - positionOnGround.y) * 10)) + 1;
            yield return null;
        }
    }

    public override void deathProcedure()
    {
        StopCoroutine(beamAttackRoutine);
        StopCoroutine(attackLoopRoutine);
        animator.enabled = false;
        spriteRenderer.sprite = emptyHelmet;
        LeanTween.move(this.gameObject, transform.position + new Vector3(0, -2.5f), 1f);
        LeanTween.value(2.5f, 0, 1f).setEaseOutQuad().setOnUpdate((float val) => { positionOnGround = new Vector3(0, val, 0); });
        takeDamageCollider.enabled = false;
        bossScript.HelmetDied();
        deathAudio.Play();
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        takeDamageAudio.Play();
        SpawnArtifactKillsAndGoOnCooldown();
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
