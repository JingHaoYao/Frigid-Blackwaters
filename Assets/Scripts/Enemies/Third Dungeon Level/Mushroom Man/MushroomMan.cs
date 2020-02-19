using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMan : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private int whatMushroomMan;
    public GameObject deadMushroom;
    public GameObject mushRoomProjectile;
    private bool isAttacking = false;
    [SerializeField] private LayerMask rayCastLayerMask;
    public Sprite[] viewSprites;
    private int whatView;
    private int mirror = 1;
    public GameObject foamParticles;
    private bool bloomed = false;

    private void Start()
    {
        animator.enabled = false;
        StartCoroutine(playAttackAnimation(2.5f));
    }

    private void Update()
    {
        float angleToShip = (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        pickView(angleToShip);
        if(animator.enabled == false)
        {
            spriteRenderer.sprite = viewSprites[whatView - 1];
            transform.localScale = new Vector3(4.5f * mirror, 4.5f);
        }

    }

    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 3;
            mirror = -1;
        }
        else if (angleOrientation >= 180 && angleOrientation < 240)
        {
            whatView = 2;
            mirror = -1;
        }
        else if (angleOrientation >= 240 && angleOrientation < 300)
        {
            whatView = 1;
            mirror = 1;
        }
        else
        {
            whatView = 2;
            mirror = 1;
        }
    }

    IEnumerator playAttackAnimation(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        transform.localScale = new Vector3(4.5f * mirror, 4.5f);
        animator.enabled = true;
        animator.SetInteger("WhatView", whatView);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(4 / 12f);
        attackAudio.Play();

        float maxDistance = 0;
        Vector3 direction = Vector3.up;
        for (int i = 0; i < 8; i++)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.35f, new Vector2(Mathf.Sin(i * 45 * Mathf.Deg2Rad), Mathf.Cos(i * 45 * Mathf.Deg2Rad)), 20, rayCastLayerMask);
            float distanceToHit = Vector2.Distance(transform.position, hit.point);
            if (distanceToHit > maxDistance)
            {
                direction = new Vector3(Mathf.Sin(i * 45 * Mathf.Deg2Rad), Mathf.Cos(i * 45 * Mathf.Deg2Rad));
                maxDistance = distanceToHit;
            }
            else if(distanceToHit == maxDistance)
            {
                if(Random.Range(0, 2) == 1)
                {
                    direction = new Vector3(Mathf.Sin(i * 45 * Mathf.Deg2Rad), Mathf.Cos(i * 45 * Mathf.Deg2Rad));
                }
            }
        }

        yield return new WaitForSeconds(1 / 12f);

        StartCoroutine(spawnFoam(Mathf.Atan2(direction.y - transform.position.y, direction.x - transform.position.x) * Mathf.Rad2Deg, Mathf.Clamp(maxDistance, 0, 6) / speed));
        LeanTween.move(this.gameObject, transform.position + direction * Mathf.Clamp(maxDistance, 0, 6), Mathf.Clamp(maxDistance, 0, 6) / speed)
            .setEaseInOutCubic()
            .setOnComplete(() => StartCoroutine(playAttackAnimation(1)))
            .setOnUpdate((float val) => {
                if (stopAttacking == false) {
                    LeanTween.cancel(this.gameObject);
                }
            });

        if (stopAttacking == false)
        {
            if (whatMushroomMan == 0)
            {
                blueAttack();
            }
            else if (whatMushroomMan == 1)
            {
                purpleAttack();
            }
            else
            {
                greenAttack();
            }
        }

        yield return new WaitForSeconds(4 / 12f);
        animator.enabled = false;
    }

    IEnumerator spawnFoam(float whatAngle, float duration)
    {
        float currDuration = 0;
        while (currDuration < duration)
        {
            Instantiate(foamParticles, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            yield return new WaitForSeconds(0.05f);
            currDuration += 0.05f;
        }
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if(newStatus.name == "Bloom Status Effect" || newStatus.name == "Bloom Status Effect (Clone)")
        {
            bloomed = true;
        }
    }

    void blueAttack()
    {
        for(int i = 0; i < 8; i++)
        {
            float angle = i * 45;
            GameObject projectileInstant = Instantiate(mushRoomProjectile, transform.position + Vector3.up, Quaternion.identity);
            projectileInstant.GetComponent<BasicProjectile>().angleTravel = angle;
            projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            if (bloomed)
            {
                DamageHitBox hitBox = projectileInstant.GetComponent<DamageHitBox>();
                hitBox.damageAmount = Mathf.RoundToInt(hitBox.damageAmount * 1.5f);
            }
        }
    }

    void purpleAttack()
    {
        for(int i = 0; i < 4; i++)
        {
            float angle = i * 90 + 45;
            GameObject projectileInstant = Instantiate(mushRoomProjectile, transform.position + Vector3.up, Quaternion.identity);
            projectileInstant.GetComponent<BasicProjectile>().angleTravel = angle - 5f;
            projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            if (bloomed)
            {
                DamageHitBox hitBox = projectileInstant.GetComponent<DamageHitBox>();
                hitBox.damageAmount = Mathf.RoundToInt(hitBox.damageAmount * 1.5f);
            }
            projectileInstant = Instantiate(mushRoomProjectile, transform.position + Vector3.up, Quaternion.identity);
            projectileInstant.GetComponent<BasicProjectile>().angleTravel = angle + 5f;
            projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            if (bloomed)
            {
                DamageHitBox hitBox = projectileInstant.GetComponent<DamageHitBox>();
                hitBox.damageAmount = Mathf.RoundToInt(hitBox.damageAmount * 1.5f);
            }
        }
    }

    void greenAttack()
    {
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90;
            GameObject projectileInstant = Instantiate(mushRoomProjectile, transform.position + Vector3.up, Quaternion.identity);
            projectileInstant.GetComponent<BasicProjectile>().angleTravel = angle - 5f;
            projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            if (bloomed)
            {
                DamageHitBox hitBox = projectileInstant.GetComponent<DamageHitBox>();
                hitBox.damageAmount = Mathf.RoundToInt(hitBox.damageAmount * 1.5f);
            }
            projectileInstant = Instantiate(mushRoomProjectile, transform.position + Vector3.up, Quaternion.identity);
            projectileInstant.GetComponent<BasicProjectile>().angleTravel = angle + 5f;
            projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            if (bloomed)
            {
                DamageHitBox hitBox = projectileInstant.GetComponent<DamageHitBox>();
                hitBox.damageAmount = Mathf.RoundToInt(hitBox.damageAmount * 1.5f);
            }
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
        Instantiate(deadMushroom, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        damageAudio.Play();
        StartCoroutine(hitFrame());
    }
}
