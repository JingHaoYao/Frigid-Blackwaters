using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyrotheumGolem : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    private float travelAngle;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private GameObject pyroGolemDeath;
    bool isAttacking = false;
    int prevView = -1;
    int whatView = 1;
    int mirror = 1;
    public GameObject pyrotheumProjectile;
    float attackPeriod = 0;
    Vector3 targetPosition;
    float distanceSpeedBonus = 0;
    Camera mainCamera;

    void pickView()
    {
        pickView(angleToShip());
        if (prevView != whatView)
        {
            prevView = whatView;
            animator.SetTrigger("Idle" + whatView);
        }
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 4;
            mirror = -1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 4;
            mirror = 1;
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

    void moveTowards(float direction, float speedToTravel)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speedToTravel;
    }

    IEnumerator dive()
    {
        isAttacking = true;
        float angleAttack = angleToShip();
        animator.SetTrigger("TeleportStart");

        rigidBody2D.velocity = Vector3.zero;

        yield return new WaitForSeconds(23 / 12f);

        transform.position = pickRandomPositionNextToShip();

        animator.SetTrigger("TeleportExit");
        yield return new WaitForSeconds(9 / 12f);
        attackAudio.Play();

        if (stopAttacking == false)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject projectileInstant = Instantiate(pyrotheumProjectile, transform.position + Vector3.up, Quaternion.identity);
                projectileInstant.GetComponent<PyrotheumProjectile>().angleTravel = i * 60;
                projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }

        yield return new WaitForSeconds(12 / 12f);

        isAttacking = false;
        pickView(angleToShip());
        prevView = whatView;
        animator.SetTrigger("Idle" + whatView);
    }

    void travelLocation()
    {
        if (isAttacking == false)
        {
            pickView();
            transform.localScale = new Vector3(3 * mirror, 3);

            attackPeriod += Time.deltaTime;
            if (attackPeriod > 2 && stopAttacking == false)
            {
                StartCoroutine(dive());
                attackPeriod = 0;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(mainLoop());
        mainCamera = Camera.main;
    }

    IEnumerator mainLoop()
    {
        while (true)
        {
            travelLocation();
            yield return null;
        }
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    Vector3 pickRandomPositionNextToShip()
    {
        float randomAngle = Random.Range(0, 360);
        Vector3 potentialLocation = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)) * Random.Range(3.0f, 4.0f);

        return new Vector3(Mathf.Clamp(potentialLocation.x, mainCamera.transform.position.x - 7, mainCamera.transform.position.x + 7), Mathf.Clamp(potentialLocation.y, mainCamera.transform.position.y - 7, mainCamera.transform.position.y + 6));
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
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
        Instantiate(pyroGolemDeath, transform.position, Quaternion.identity);
        spawnEndingPyrotheumAttacks();
        Destroy(this.gameObject);
    }

    void spawnEndingPyrotheumAttacks()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject pyrotheumProjectileInstant = Instantiate(pyrotheumProjectile, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            pyrotheumProjectile.GetComponent<PyrotheumProjectile>().angleTravel = i * 60;
        }
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
