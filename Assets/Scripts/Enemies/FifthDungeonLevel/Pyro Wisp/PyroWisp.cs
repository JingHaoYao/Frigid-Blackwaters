using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroWisp : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    private float travelAngle;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private GameObject pyroWispDeath;
    bool isAttacking = false;
    int prevView = -1;
    int whatView = 1;
    int mirror = 1;
    public GameObject pyrotheumProjectile;
    [SerializeField] GameObject damagingHitBox;
    float attackPeriod = 0;
    Vector3 targetPosition;
    float angleOffset = 0;
    float distanceSpeedBonus = 0;

    void pickView()
    {
        pickView(angleToShip());
        if(prevView != whatView)
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
            mirror = 1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 4;
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

    void moveTowards(float direction, float speedToTravel)
    {
        if (rigidBody2D != null)
        {
            rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speedToTravel;
        }
    }

    IEnumerator dive()
    {
        isAttacking = true;
        float angleAttack = angleToShip();
        pickView(angleAttack);
        animator.SetTrigger("Attack" + whatView);

        rigidBody2D.velocity = Vector3.zero;
        damagingHitBox.SetActive(true);
        attackAudio.Play();

        LeanTween.value(0, 10, 0.5f).setOnUpdate((float val) => moveTowards(angleAttack, this.speed + val));

        yield return new WaitForSeconds(0.5f);

        LeanTween.value(10, 4, 0.333f).setOnUpdate((float val) => moveTowards(angleAttack, this.speed + val));

        yield return new WaitForSeconds(0.333f);

        angleOffset = Mathf.Atan2(transform.position.y - PlayerProperties.playerShipPosition.y, transform.position.x - PlayerProperties.playerShipPosition.x) * Mathf.Rad2Deg;
        damagingHitBox.SetActive(false);
        isAttacking = false;
        pickView(angleToShip());
        prevView = whatView;
        animator.SetTrigger("Idle" + whatView);
    }

    void travelLocation()
    {
        if (isAttacking == false)
        {
            angleOffset += Time.deltaTime * 120;

            if (angleOffset > 360)
            {
                angleOffset = 0;
            }

            targetPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleOffset * Mathf.Deg2Rad), Mathf.Sin(angleOffset * Mathf.Deg2Rad)) * 5;

            distanceSpeedBonus = Vector2.Distance(transform.position, targetPosition);

            if (Vector2.Distance(transform.position, targetPosition) > 0.2f)
            {
                moveTowards(Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg, speed + distanceSpeedBonus);
            }
            else
            {
                transform.position = targetPosition;
            }

            pickView();
            transform.localScale = new Vector3(3 * mirror, 3);

            attackPeriod += Time.deltaTime;
            if (attackPeriod > 3 && stopAttacking == false)
            {
                StartCoroutine(dive());
                attackPeriod = 0;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(mainLoop());
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
        StopAllCoroutines();
        LeanTween.cancel(this.gameObject);
        Instantiate(pyroWispDeath, transform.position, Quaternion.identity);
        spawnEndingPyrotheumAttacks();
        Destroy(this.gameObject);
    }

    void spawnEndingPyrotheumAttacks()
    {
        for(int i = 0; i < 6; i++)
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
