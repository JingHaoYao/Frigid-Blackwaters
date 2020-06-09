using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanChampion : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    public Sprite[] viewSprites;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private float travelAngle;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    [SerializeField] private AudioSource throwSpearAudio;
    [SerializeField] private AudioSource lanceDashAudio;
    [SerializeField] private AudioSource damageAudio;
    bool isAttacking = false;
    int whatView = 1;
    int mirror = 1;
    [SerializeField] GameObject spearProjectile;
    [SerializeField] GameObject damageCollider;
    [SerializeField] InvisibilityEnemyController invisController;
    Camera mainCamera;
    float attackPeriod = 0;

    bool fogMode = false;

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Fogged Effect" || newStatus.name == "Fogged Effect(Clone)")
        {
            invisController.FogActivated();
            attackPeriod = 0;
            fogMode = true;
        }
    }

    public override void statusRemoved(EnemyStatusEffect removedStatus)
    {
        if (removedStatus.name == "Fogged Effect" || removedStatus.name == "Fogged Effect(Clone)")
        {
            invisController.FogDeActivated();
            fogMode = false;
        }
    }

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0 && invisController.isUnderLight)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * speed / 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void spawnProjectiles(float angleAttack)
    {
        GameObject instant = Instantiate(spearProjectile, transform.position + new Vector3(Mathf.Cos(angleAttack * Mathf.Deg2Rad), Mathf.Sin(angleAttack * Mathf.Deg2Rad) + 1.5f) * 0.5f, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        instant.GetComponent<BasicProjectile>().angleTravel = angleAttack;
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction, float speedModifier)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speedModifier;
    }

    float cardinalizeDirections(float angle)
    {
        if (angle > 22.5f && angle <= 67.5f)
        {
            return 45;
        }
        else if (angle > 67.5f && angle <= 112.5f)
        {
            return 90;
        }
        else if (angle > 112.5f && angle <= 157.5f)
        {
            return 135;
        }
        else if (angle > 157.5f && angle <= 202.5f)
        {
            return 180;
        }
        else if (angle > 202.5f && angle <= 247.5f)
        {
            return 225;
        }
        else if (angle > 247.5 && angle < 292.5)
        {
            return 270;
        }
        else if (angle > 292.5 && angle < 337.5)
        {
            return 315;
        }
        else
        {
            return 0;
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
        animator.SetInteger("WhatView", whatView);
    }

    private void Start()
    {
        animator.enabled = false;
        mainCamera = Camera.main;
    }

    void Update()
    {
        spawnFoam();
        travelLocation();
    }

    IEnumerator launchSpear()
    {
        animator.enabled = true;
        isAttacking = true;
        float angleAttack = angleToShip();
        pickView(angleToShip());
        transform.localScale = new Vector3(3 * mirror, 3);

        animator.SetTrigger("ThrowSpear");

        yield return new WaitForSeconds(6 / 12f);

        throwSpearAudio.Play();
        if (stopAttacking == false)
        {
            spawnProjectiles(angleAttack);
        }

        yield return new WaitForSeconds(4/12);

        pickView(angleToShip());
        transform.localScale = new Vector3(3 * mirror, 3);
        spriteRenderer.sprite = viewSprites[whatView - 1];
        isAttacking = false;
        animator.enabled = false;
    }

    IEnumerator swordDash()
    {
        animator.enabled = true;
        isAttacking = true;
        float angleAttack = angleToShip();
        transform.localScale = new Vector3(3 * mirror, 3);

        animator.SetTrigger("ChargeLance");

        yield return new WaitForSeconds(5 / 12f);

        damageCollider.SetActive(true);
        
        damageCollider.transform.rotation = Quaternion.Euler(0, 0, angleAttack + (transform.localScale.x < 0 ? 180 : 0));

        lanceDashAudio.Play();

        yield return new WaitForSeconds(3 / 12);

        damageCollider.SetActive(false);

        yield return new WaitForSeconds(2 / 12f);
        animator.enabled = false;
        isAttacking = false;
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = PlayerProperties.playerShipPosition;
        Vector3 targetPos = Vector3.zero;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);


        if (fogMode == false)
        {
            if (isAttacking == false)
            {
                if (attackPeriod < 1)
                {
                    attackPeriod += Time.deltaTime;
                }
                else
                {
                    StartCoroutine(launchSpear());
                    attackPeriod = 0;
                }
            }

            if (path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 4 && isAttacking == false)
            {
                moveTowards(travelAngle, this.speed);

                pickSpritePeriod += Time.deltaTime;

                if (pickSpritePeriod >= 0.2f)
                {
                    pickView(travelAngle);
                    pickSpritePeriod = 0;
                    spriteRenderer.sprite = viewSprites[whatView - 1];
                    transform.localScale = new Vector3(3 * mirror, 3);
                }
            }
            else
            {
                rigidBody2D.velocity = Vector3.zero;
            }
        }
        else
        {
            if (path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 2)
            {
                moveTowards(travelAngle, this.speed + 2);

                pickSpritePeriod += Time.deltaTime;
                if (pickSpritePeriod >= 0.2f)
                {
                    pickView(travelAngle);
                    pickSpritePeriod = 0;
                    spriteRenderer.sprite = viewSprites[whatView - 1];
                    transform.localScale = new Vector3(3 * mirror, 3);
                }
            }
            else
            {
                if (!isAttacking)
                {
                    StartCoroutine(swordDash());
                }
                rigidBody2D.velocity = Vector3.zero;
            }
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
        invisController.hideRendererAfterHit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void deathProcedure()
    {
        GameObject deadPirate = Instantiate(deadSpearman, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
