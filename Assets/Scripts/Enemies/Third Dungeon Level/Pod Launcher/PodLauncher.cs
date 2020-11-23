﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodLauncher : Enemy
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource damageAudio;
    [SerializeField] AudioSource attackAudio;
    private List<AStarNode> path;

    private float foamTimer = 0;
    public GameObject waterFoam;

    public GameObject deadSkele;

    private float angleToShip;

    private float attackPeriod = 2;

    public GameObject explosivePod;

    private bool bloomed = false;
    private bool isAttacking = false;

    [SerializeField] private AStarPathfinding aStarPathfinding;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * speed / 3f)
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
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

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = PlayerProperties.playerShipPosition;
        Vector3 targetPos = PlayerProperties.playerShipPosition;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (path != null && path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 4f)
        {
            moveTowards(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }
    }

    void Update()
    {
        angleToShip = (Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        spawnFoam();

        if (attackPeriod > 0)
        {
            travelLocation();
            attackPeriod -= Time.deltaTime;
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            if (isAttacking == false)
            {
                isAttacking = true;
                StartCoroutine(launchPod());
            }
        }
    }

    IEnumerator launchPod()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(6f / 12f);
        attackAudio.Play();
        GameObject podInstant = Instantiate(explosivePod, transform.position + Vector3.up * 1.2f, Quaternion.identity);
        podInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        yield return new WaitForSeconds(10f / 12f);
        isAttacking = false;
        attackPeriod = bloomed ? 1 : 3;
        animator.SetTrigger("Idle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Bloom Status Effect" || newStatus.name == "Bloom Status Effect(Clone)")
        {
            bloomed = true;
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
        GameObject dead = Instantiate(deadSkele, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        damageAudio.Play();
        StartCoroutine(hitFrame());
    }
}
