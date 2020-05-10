using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanBolaThrower : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private float travelAngle;
    private float speedBonus;
    private float pokePeriod = 1.5f;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    bool isAttacking = false;
    int whatView = 1;
    int mirror = 1;
    [SerializeField] GameObject bola;
    int prevView = 0;
    private float attackPeriod = 0;
    [SerializeField] InvisibilityEnemyController invisController;

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

    void spawnBola(float angle)
    {
        GameObject bolaInstant = Instantiate(bola, transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle) + 0.7f) * 0.5f, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg));
        bolaInstant.GetComponent<FrogmanBola>().travelAngle = angle * Mathf.Rad2Deg;
        bolaInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        if (!invisController.isUnderLight)
        {
            bolaInstant.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            LeanTween.alpha(bolaInstant, 1, 0.5f);
        }
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * (speed + speedBonus);
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
        transform.localScale = new Vector3(mirror * 3, 3);
    }

    void pickIdleAnimation()
    {
        pickView(angleToShip());
        if(whatView != prevView)
        {
            prevView = whatView;
            animator.SetInteger("WhatView", whatView);
            animator.SetTrigger("Idle");
        }
    }

    void Update()
    {
        spawnFoam();
        travelLocation();
    }

    IEnumerator launchBolaAttack()
    {
        isAttacking = true;
        float throwAngle = angleToShip();
        pickView(throwAngle);
        animator.SetInteger("WhatView", whatView);
        animator.SetTrigger("Throw");
        yield return new WaitForSeconds(6 / 12f);
        attackAudio.Play();
        if (stopAttacking == false)
        {
            spawnBola(throwAngle * Mathf.Deg2Rad);
        }
        yield return new WaitForSeconds(3 / 12f);
        isAttacking = false;
        pickView(angleToShip());
        animator.SetTrigger("Idle");
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Fogged Effect" || newStatus.name == "Fogged Effect(Clone)")
        {
            invisController.FogActivated();
        }
    }

    public override void statusRemoved(EnemyStatusEffect removedStatus)
    {
        if (removedStatus.name == "Fogged Effect" || removedStatus.name == "Fogged Effect(Clone)")
        {
            invisController.FogDeActivated();
        }
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = PlayerProperties.playerShipPosition;
        Vector3 targetPos = transform.position;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(PlayerProperties.playerShipPosition, transform.position) > 3 && isAttacking == false)
        {
            moveTowards(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }

        if(attackPeriod < 3.5)
        {
            attackPeriod += Time.deltaTime;
        }
        else
        {
            StartCoroutine(launchBolaAttack());
            attackPeriod = 0;
        }

        if(isAttacking == false)
        {
            pickIdleAnimation();
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
