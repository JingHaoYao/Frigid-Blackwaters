using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheonixSpirit : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private float travelAngle;
    List<AStarNode> path;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private AudioSource reviveAudio;
    [SerializeField] private AudioSource deathAudio;
    bool isAttacking = false;
    int whatView = 1;
    int mirror = 1;
    [SerializeField] GameObject fireProjectile;
    int prevView = 0;
    private float attackPeriod = 0;

    [SerializeField] Collider2D largeHitbox, smallHitbox;
    [SerializeField] GameObject shadowObject;
    private bool eggForm = false;
    Vector3 targetPos;

    Coroutine mainLoopRoutine;
    Coroutine currentAttackRoutine;
    Camera mainCamera;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
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
        transform.localScale = new Vector3(mirror * 4, 4);
    }

    void pickIdleAnimation(float angle)
    {
        pickView(angle);
        if (whatView != prevView)
        {
            prevView = whatView;
            animator.SetTrigger("Idle" + whatView);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        pickAwayPosition();
        mainLoopRoutine = StartCoroutine(mainLoop());
        smallHitbox.enabled = false;
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void pickAwayPosition()
    {
        float angle = Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x);
        angle += Mathf.PI;

        Vector3 potentialPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(2.0f, 4.0f);
        targetPos = new Vector3(Mathf.Clamp(potentialPosition.x, mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8), Mathf.Clamp(potentialPosition.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8));
    }

    IEnumerator fireBolts()
    {
        pickView(angleToShip());
        animator.SetTrigger("Attack" + whatView);
        attackAudio.Play();
        isAttacking = true;
        yield return new WaitForSeconds(5/ 12f);
        float angleAttack = angleToShip();
        GameObject projectileInstant = Instantiate(fireProjectile, transform.position + Vector3.up * 2 + new Vector3(Mathf.Cos(angleAttack * Mathf.Deg2Rad), Mathf.Sin(angleAttack * Mathf.Deg2Rad)), Quaternion.identity);
        projectileInstant.GetComponent<PyrotheumProjectile>().angleTravel = angleToShip();
        projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        yield return new WaitForSeconds(2 / 12f);
        angleAttack = angleToShip();
        projectileInstant = Instantiate(fireProjectile, transform.position + Vector3.up * 2 + new Vector3(Mathf.Cos(angleAttack * Mathf.Deg2Rad), Mathf.Sin(angleAttack * Mathf.Deg2Rad)), Quaternion.identity);
        projectileInstant.GetComponent<PyrotheumProjectile>().angleTravel = angleToShip();
        projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        yield return new WaitForSeconds(5 / 12f);
        isAttacking = false;
        prevView = -1;
        pickIdleAnimation(angleToShip());
    }

    IEnumerator mainLoop()
    {
        while (true)
        {
            path = aStarPathfinding.seekPath;
            aStarPathfinding.target = targetPos;
            Vector3 nodePos = targetPos;

            if (path.Count > 0)
            {
                AStarNode pathNode = path[0];
                nodePos = pathNode.nodePosition;
            }

            float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(nodePos.y - (transform.position.y + 0.4f), nodePos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

            if (path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && isAttacking == false)
            {
                moveTowards(travelAngle);

                if (isAttacking == false)
                {
                    pickIdleAnimation(travelAngle);
                }
            }
            else
            {
                rigidBody2D.velocity = Vector3.zero;

                if (isAttacking == false && Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 3)
                {
                    pickAwayPosition();
                }

                if (isAttacking == false)
                {
                    pickIdleAnimation(angleToShip());
                }
            }

            if (attackPeriod < 3)
            {
                attackPeriod += Time.deltaTime;
            }
            else
            {
                currentAttackRoutine = StartCoroutine(fireBolts());
                attackPeriod = 0;
            }


            yield return null;
        }
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
            if (!eggForm)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage, false);
            }
            else
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
        }
    }

    IEnumerator turnToEggForm()
    {
        animator.SetTrigger("PheonixDeath");
        yield return new WaitForSeconds(15 / 12f);
        eggForm = true;
        smallHitbox.enabled = true;
        health = Mathf.RoundToInt(maxHealth / 2f);

        yield return new WaitForSeconds(6f);

        smallHitbox.enabled = false;
        animator.SetTrigger("PheonixRevive");
        reviveAudio.Play();

        yield return new WaitForSeconds(15 / 12f);
        prevView = -1;
        mainLoopRoutine = StartCoroutine(mainLoop());
        largeHitbox.enabled = true;
        isAttacking = false;
        eggForm = false;
        health = maxHealth;
    }

    public override void deathProcedure()
    {
        if (!eggForm)
        {
            deathAudio.Play();
            StopCoroutine(mainLoopRoutine);
            StopCoroutine(currentAttackRoutine);
            StartCoroutine(turnToEggForm());
            rigidBody2D.velocity = Vector3.zero;
            largeHitbox.enabled = false;
        }
        else
        {
            StopAllCoroutines();
            LeanTween.alpha(shadowObject, 0, 0.5f);
            animator.SetTrigger("PheonixEggDeath");
            Destroy(this.gameObject, 11 / 12f);
        }
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
