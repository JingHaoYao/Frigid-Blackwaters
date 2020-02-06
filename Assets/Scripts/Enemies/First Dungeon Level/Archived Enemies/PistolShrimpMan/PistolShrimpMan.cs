using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpMan : Enemy {
    public Sprite facingDownLeft, facingDown, facingDownRight, facingUpLeft, facingUp, facingUpRight;
    Rigidbody2D rigidBody2D;
    public GameObject shrimpShot, deadShrimp;
    SpriteRenderer spriteRenderer;
    public float travelSpeed = 2;
    GameObject playerShip;
    Animator animator;
    private bool isShooting;
    private float coolDownPeriod = 5;
    private float coolDownThreshold = 5;
    private float angleFire = 0;
    private float moveAngle = 0;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    public GameObject bloodSplatter;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * travelSpeed / 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void pickSprite(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation <= 75)
        {
            spriteRenderer.sprite = facingUpRight;
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            spriteRenderer.sprite = facingUp;
        }
        else if (angleOrientation > 105 && angleOrientation <= 180)
        {
            spriteRenderer.sprite = facingUpLeft;
        }
        else if (angleOrientation > 180 && angleOrientation <= 255)
        {
            spriteRenderer.sprite = facingDownLeft;
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {

            spriteRenderer.sprite = facingDown;
        }
        else
        {
            spriteRenderer.sprite = facingDownRight;
        }
    }

    IEnumerator fireShot()
    {
        angleFire = Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x);
        float spriteFireAngle = (360 + angleFire * Mathf.Rad2Deg) % 360;
        pickSprite(spriteFireAngle);
        animator.enabled = true;
        this.GetComponents<AudioSource>()[1].Play();
        if (spriteRenderer.sprite == facingDownLeft)
        {
            animator.SetTrigger("Attack1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Attack2");
        }
        else if (spriteRenderer.sprite == facingDownRight)
        {
            animator.SetTrigger("Attack3");
        }
        else if (spriteRenderer.sprite == facingUpRight)
        {
            animator.SetTrigger("Attack4");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Attack5");
        }
        else
        {
            animator.SetTrigger("Attack6");
        }
        yield return new WaitForSeconds(6f / 12f);
        if (spriteRenderer.sprite == facingDownLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject instantiatedRound = Instantiate(shrimpShot, transform.position + new Vector3(-0.737f, 0.5f, 0), Quaternion.Euler(0, 0, (angleFire) * Mathf.Rad2Deg - 15 + 15*i));
                instantiatedRound.GetComponent<PistolShrimpShot>().angleTravel = ((angleFire) * Mathf.Rad2Deg - 15 + 15 * i) * Mathf.Deg2Rad;
                instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject instantiatedRound = Instantiate(shrimpShot, transform.position + new Vector3(0, 0.4f, 0), Quaternion.Euler(0, 0, (angleFire) * Mathf.Rad2Deg - 15 + 15 * i));
                instantiatedRound.GetComponent<PistolShrimpShot>().angleTravel = ((angleFire) * Mathf.Rad2Deg - 15 + 15 * i) * Mathf.Deg2Rad;
                instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else if (spriteRenderer.sprite == facingDownRight)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject instantiatedRound = Instantiate(shrimpShot, transform.position + new Vector3(0.737f, 0.5f, 0), Quaternion.Euler(0, 0, (angleFire) * Mathf.Rad2Deg - 15 + 15 * i));
                instantiatedRound.GetComponent<PistolShrimpShot>().angleTravel = ((angleFire) * Mathf.Rad2Deg - 15 + 15 * i) * Mathf.Deg2Rad;
                instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else if (spriteRenderer.sprite == facingUpRight)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject instantiatedRound = Instantiate(shrimpShot, transform.position + new Vector3(0.6f, 0.74f, 0), Quaternion.Euler(0, 0, (angleFire) * Mathf.Rad2Deg - 15 + 15 * i));
                instantiatedRound.GetComponent<PistolShrimpShot>().angleTravel = ((angleFire) * Mathf.Rad2Deg - 15 + 15 * i) * Mathf.Deg2Rad;
                instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject instantiatedRound = Instantiate(shrimpShot, transform.position + new Vector3(0, 0.6f, 0), Quaternion.Euler(0, 0, (angleFire) * Mathf.Rad2Deg - 15 + 15 * i));
                instantiatedRound.GetComponent<PistolShrimpShot>().angleTravel = ((angleFire) * Mathf.Rad2Deg - 15 + 15 * i) * Mathf.Deg2Rad;
                instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject instantiatedRound = Instantiate(shrimpShot, transform.position + new Vector3(-0.6f, 0.74f, 0), Quaternion.Euler(0, 0, (angleFire) * Mathf.Rad2Deg - 15 + 15 * i));
                instantiatedRound.GetComponent<PistolShrimpShot>().angleTravel = ((angleFire) * Mathf.Rad2Deg - 15 + 15 * i) * Mathf.Deg2Rad;
                instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        yield return new WaitForSeconds(6f / 12f);
        animator.enabled = false;
        isShooting = false;
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

    void findPositionShoot()
    {
        if (coolDownPeriod > 0)
        {
            coolDownPeriod -= Time.deltaTime;
        }
        else
        {
            coolDownPeriod = 0;
        }

        if (Vector2.Distance(playerShip.transform.position, transform.position) > 9)
        {
            AStarNode pathNode = path[0];
            Vector3 targetPos = pathNode.nodePosition;
            moveAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
            if (isShooting == false)
            {
                moveTowards(moveAngle);
            }
        }
        else
        {
            float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            pickSprite(angleToShip);
            moveAngle = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            rigidBody2D.velocity = Vector3.zero;
            if (coolDownPeriod <= 0 && stopAttacking == false)
            {
                isShooting = true;
                StartCoroutine(fireShot());
                coolDownPeriod = coolDownThreshold;
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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
        animator = GetComponent<Animator>();
        animator.enabled = false;
        updateSpeed(travelSpeed);
    }

    void Update()
    {
        this.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
        path = GetComponent<AStarPathfinding>().seekPath;
        pickRendererLayer();
        findPositionShoot();
        pickSpritePeriod += Time.deltaTime;

        if (isShooting == false)
        {
            //makes sure that sprites only update every so often
            if (pickSpritePeriod > 0.2f)
            {
                pickSpritePeriod = 0;
                pickSprite(moveAngle);
            }
        }
        spawnFoam();
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    int whatView()
    {
        if (spriteRenderer.sprite == facingDownLeft)
        {
            return 1;
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            return 2;
        }
        else if (spriteRenderer.sprite == facingDownRight)
        {
            return 3;
        }
        else if (spriteRenderer.sprite == facingUpRight)
        {
            return 4;
        }
        else if(spriteRenderer.sprite == facingUp)
        {
            return 5;
        }
        else
        {
            return 6;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            this.GetComponents<AudioSource>()[0].Play();
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
            StartCoroutine(hitFrame());
            Instantiate(bloodSplatter, collision.transform.position, Quaternion.identity);
        }
    }

    public override void deathProcedure()
    {
        GameObject dead = Instantiate(deadShrimp, transform.position, Quaternion.identity);
        dead.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
        dead.GetComponent<DeadEnemyScript>().whatView = whatView();
        dead.transform.localScale = transform.localScale;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {

    }
}
