using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NettleBearer : Enemy
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource damageAudio;
    [SerializeField] AudioSource attackAudio;
    private List<AStarNode> path;
    public Sprite[] viewSprites;

    private float foamTimer = 0;
    public GameObject waterFoam;

    public GameObject deadSkele;

    private int whatView = 0;
    private int mirror = 1;

    private float pickSpritePeriod = 0;
    private float angleToShip;

    private float attackPeriod = 2;

    public GameObject armedNettle;

    private bool bloomed = false;
    private bool isAttacking = false;

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

    void travelLocation()
    {
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = PlayerProperties.playerShipPosition;
        Vector3 targetPos = Vector3.zero;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (path != null && path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 3f)
        {
            moveTowards(travelAngle);
            pickView(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            pickView(angleToShip);
        }

        transform.localScale = new Vector3(4f * mirror, 4f);
        if (pickSpritePeriod >= 0.2f)
        {
            pickSpritePeriod = 0;
            spriteRenderer.sprite = viewSprites[whatView - 1];
        }
    }

    private void Start()
    {
        animator.enabled = false;
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
            if(isAttacking == false)
            {
                isAttacking = true;
                StartCoroutine(launchNettle());
            }
        }
    }

    IEnumerator launchNettle()
    {
        animator.enabled = true;
        animator.SetInteger("WhatView", whatView);
        animator.SetTrigger("Attack");
        float angleToAttack = angleToShip;
        yield return new WaitForSeconds(3f / 12f);
        attackAudio.Play();

        if (!bloomed)
        {
            GameObject nettleInstant = Instantiate(armedNettle, transform.position + Vector3.up * 0.5f, Quaternion.Euler(0, 0, angleToAttack));
            nettleInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            nettleInstant.GetComponent<ArmedNettle>().angleTravelInDeg = angleToAttack;
        }
        else
        {
            for(int i = 0; i < 3; i++)
            {
                float angleAttack = angleToAttack - 10 + i * 10;
                GameObject nettleInstant = Instantiate(armedNettle, transform.position + Vector3.up * 0.5f, Quaternion.Euler(0, 0, angleAttack));
                nettleInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                nettleInstant.GetComponent<ArmedNettle>().angleTravelInDeg = angleAttack;
            }
        }

        yield return new WaitForSeconds(6 / 12f);
        animator.enabled = false;
        attackPeriod = 2;
        isAttacking = false;
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
        if (newStatus.name == "Bloom Status Effect" || newStatus.name == "Bloom Status Effect (Clone)")
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
