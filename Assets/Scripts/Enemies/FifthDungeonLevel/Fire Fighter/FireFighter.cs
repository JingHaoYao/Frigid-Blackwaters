using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFighter : Enemy
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
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource pyrotheumAttackAudio;
    [SerializeField] private AudioSource flameAttackAudio;
    [SerializeField] GameObject pyrotheumProjectile, fireBallProjectile;
    bool isAttacking = false;
    int whatView = 1;
    int mirror = 1;
    private float offset = 0;
    Camera mainCamera;
    bool pyrotheumAttack = false;
    float attackPeriod = 0;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
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

        if (path != null && path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(PlayerProperties.playerShipPosition, transform.position) > 4 && isAttacking == false)
        {
            moveTowards(travelAngle);

            pickViewSprite(travelAngle);
        }
        else
        {
            pickViewSprite(angleToShip());
            rigidBody2D.velocity = Vector3.zero;
        }

        if(attackPeriod < 3)
        {
            if(stopAttacking == false)
            {
                attackPeriod += Time.deltaTime;
            }
        }
        else
        {
            attackPeriod = 0;
            if (stopAttacking == false)
            {
                if (pyrotheumAttack)
                {
                    StartCoroutine(pyrotheumBlast());
                    pyrotheumAttack = false;
                }
                else
                {
                    pyrotheumAttack = true;
                    StartCoroutine(flameBurst());
                }
            }
        }
    }

    void pickViewSprite(float angle)
    {
        pickSpritePeriod += Time.deltaTime;
        if (pickSpritePeriod >= 0.2f)
        {
            pickView(angle);
            pickSpritePeriod = 0;
            spriteRenderer.sprite = viewSprites[whatView - 1];
            transform.localScale = new Vector3(4 * mirror, 4);
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

    IEnumerator pyrotheumBlast()
    {
        animator.enabled = true;
        pickView(angleToShip());
        animator.SetTrigger("PyroAttack" + whatView);
        isAttacking = true;

        yield return new WaitForSeconds(10 / 12f);
        pyrotheumAttackAudio.Play();

        if (stopAttacking == false)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject projectileInstant = Instantiate(pyrotheumProjectile, transform.position + Vector3.up, Quaternion.identity);
                projectileInstant.GetComponent<FireFighterProjectile>().Initialize(i * 45, this.gameObject, 0);
                projectileInstant = Instantiate(pyrotheumProjectile, transform.position + Vector3.up, Quaternion.identity);
                projectileInstant.GetComponent<FireFighterProjectile>().Initialize(i * 45, this.gameObject, Mathf.PI);
            }
        }

        yield return new WaitForSeconds(9 / 12f);
        animator.enabled = false;
        isAttacking = false;
    }

    IEnumerator flameBurst()
    {
        animator.enabled = true;
        animator.SetTrigger("FlameAttack");
        isAttacking = true;

        flameAttackAudio.Play();

        yield return new WaitForSeconds(8 / 12f);

        if (stopAttacking == false)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject projectileInstant = Instantiate(fireBallProjectile, transform.position, Quaternion.identity);
                projectileInstant.GetComponent<FireFighterProjectile>().Initialize(i * 45, this.gameObject, 0);
                projectileInstant = Instantiate(fireBallProjectile, transform.position, Quaternion.identity);
                projectileInstant.GetComponent<FireFighterProjectile>().Initialize(i * 45, this.gameObject, Mathf.PI);
            }
        }

        yield return new WaitForSeconds(9 / 12f);

        animator.enabled = false;
        isAttacking = false;
    }
}
