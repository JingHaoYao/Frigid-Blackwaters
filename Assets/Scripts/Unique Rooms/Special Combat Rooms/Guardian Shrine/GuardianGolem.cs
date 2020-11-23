using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianGolem : Enemy {
    public Sprite facingUpLeft, facingUp, facingLeft, facingDownLeft, facingDown;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    public GameObject blast;
    bool activatedRoom = false, isShooting = false;
    float angleToShip = 0;
    GameObject playerShip;
    public int positionAlongRoom = 0;
    public GameObject deadGolem;
    public Vector3 targetPos;
    public bool isMoving = false;
    Vector3 pastPos;
    private float foamTimer = 0;
    public GameObject waterFoam;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)((transform.position.y + transform.parent.transform.position.y) * 10);
    }

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * speed/ 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    public int whatMovement = 0;

    public void activateGolem()
    {
        this.gameObject.tag = "StrongEnemy";
        StartCoroutine(wakeUp());
    }

    IEnumerator wakeUp()
    {
        animator.enabled = true;
        animator.SetTrigger("WakeUp");
        EnemyPool.addEnemy(this);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(0.75f);
        activatedRoom = true;
        animator.enabled = false;
    }

    IEnumerator shootBlast(float angleFire)
    {
        angleFire = angleToShip;
        isShooting = true;
        animator.enabled = true;
        pickSprite(angleFire);
        this.GetComponents<AudioSource>()[0].Play();
        if (spriteRenderer.sprite == facingUpLeft)
        {
            animator.SetTrigger("Attack4");
        }
        else if(spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Attack5");
        }
        else if(spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Attack3");
        }
        else if(spriteRenderer.sprite == facingDownLeft)
        {
            animator.SetTrigger("Attack2");
        }
        else
        {
            animator.SetTrigger("Attack1");
        }
        yield return new WaitForSeconds(6f / 12f);
        GameObject blastInstant = Instantiate(blast, transform.position + new Vector3(0, 1.8f, 0) + new Vector3(Mathf.Cos(angleFire * Mathf.Deg2Rad), Mathf.Sin(angleFire * Mathf.Deg2Rad), 0) * 1.2f, Quaternion.Euler(0, 0, angleFire));
        blastInstant.GetComponent<GuardianGolemBlast>().angleTravel = angleFire;
        yield return new WaitForSeconds(4f / 12f);
        animator.enabled = false;
        isShooting = false;
    }

    public void straightCW()
    {
        if (positionAlongRoom == 0)
        {
            rigidBody2D.velocity = Vector3.up * speed;
            targetPos = pastPos + new Vector3(0, 13f);
        }
        else if (positionAlongRoom == 2)
        {
            rigidBody2D.velocity = Vector3.right * speed;
            targetPos = pastPos + new Vector3(13f, 0);
        }
        else if (positionAlongRoom == 4)
        {
            rigidBody2D.velocity = Vector3.down * speed;
            targetPos = pastPos + new Vector3(0, -13f);
        }
        else if (positionAlongRoom == 6)
        {
            rigidBody2D.velocity = Vector3.left * speed;
            targetPos = pastPos + new Vector3(-13f, 0);
        }

        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            transform.position = targetPos;
            isMoving = false;
            positionAlongRoom += 2;
            positionAlongRoom %= 8;
            rigidBody2D.velocity = Vector3.zero;
            StartCoroutine(shootBlast(angleToShip));
            pastPos = transform.position;
        }
    }

    public void straightCCW()
    {
        if (positionAlongRoom == 0)
        {
            rigidBody2D.velocity = Vector3.right * speed;
            targetPos = pastPos + new Vector3(13f, 0);
        }
        else if (positionAlongRoom == 2)
        {
            rigidBody2D.velocity = Vector3.down * speed;
            targetPos = pastPos + new Vector3(0, -13f);
        }
        else if (positionAlongRoom == 4)
        {
            rigidBody2D.velocity = Vector3.left * speed;
            targetPos = pastPos + new Vector3(-13f, 0);
        }
        else if (positionAlongRoom == 6)
        {
            rigidBody2D.velocity = Vector3.up * speed;
            targetPos = pastPos + new Vector3(0, 13f);
        }

        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            transform.position = targetPos;
            isMoving = false;
            positionAlongRoom -= 2;
            if (positionAlongRoom < 0)
            {
                positionAlongRoom = 8 + positionAlongRoom;
            }
            rigidBody2D.velocity = Vector3.zero;
            StartCoroutine(shootBlast(angleToShip));
            pastPos = transform.position;
        }
    }

    public void halfCW()
    {
        if (positionAlongRoom < 2 && positionAlongRoom >= 0)
        {
            rigidBody2D.velocity = Vector3.up * speed;
            targetPos = pastPos + new Vector3(0, 6.5f);
        }
        else if (positionAlongRoom < 4 && positionAlongRoom >= 2)
        {
            rigidBody2D.velocity = Vector3.right * speed;
            targetPos = pastPos + new Vector3(6.5f, 0);
        }
        else if (positionAlongRoom < 6 && positionAlongRoom >= 4)
        {
            rigidBody2D.velocity = Vector3.down * speed;
            targetPos = pastPos + new Vector3(0, -6.5f);
        }
        else if (positionAlongRoom < 8 && positionAlongRoom >= 6)
        {
            rigidBody2D.velocity = Vector3.left * speed;
            targetPos = pastPos + new Vector3(-6.5f, 0);
        }

        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            transform.position = targetPos;
            isMoving = false;
            positionAlongRoom += 1;
            positionAlongRoom %= 8;
            rigidBody2D.velocity = Vector3.zero;
            StartCoroutine(shootBlast(angleToShip));
            pastPos = transform.position;
        }
    }

    public void halfCCW()
    {
        if (positionAlongRoom > 6 || positionAlongRoom == 0)
        {
            rigidBody2D.velocity = Vector3.right * speed;
            targetPos = pastPos + new Vector3(6.5f, 0);
        }
        else if (positionAlongRoom <= 6 && positionAlongRoom > 4)
        {
            rigidBody2D.velocity = Vector3.up * speed;
            targetPos = pastPos + new Vector3(0, 6.5f);
        }
        else if (positionAlongRoom <= 4 && positionAlongRoom > 2)
        {
            rigidBody2D.velocity = Vector3.left * speed;
            targetPos = pastPos + new Vector3(-6.5f, 0);
        }
        else if (positionAlongRoom <= 2 && positionAlongRoom > 0)
        {
            rigidBody2D.velocity = Vector3.down * speed;
            targetPos = pastPos + new Vector3(0, -6.5f);
        }
        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            transform.position = targetPos;
            isMoving = false;
            positionAlongRoom -= 1;
            if (positionAlongRoom < 0)
            {
                positionAlongRoom = 8 + positionAlongRoom;
            }
            rigidBody2D.velocity = Vector3.zero;
            StartCoroutine(shootBlast(angleToShip));
            pastPos = transform.position;
        }
    }

    public void diagonalCW()
    {
        if (positionAlongRoom == 1)
        {
            rigidBody2D.velocity = Vector3.Normalize(new Vector3(1, 1, 0)) * speed;
            targetPos = pastPos + new Vector3(6.5f, 6.5f);
        }
        else if (positionAlongRoom == 3)
        {
            rigidBody2D.velocity = Vector3.Normalize(new Vector3(1, -1, 0)) * speed;
            targetPos = pastPos + new Vector3(6.5f, -6.5f);
        }
        else if (positionAlongRoom == 5)
        {
            rigidBody2D.velocity = Vector3.Normalize(new Vector3(-1, -1, 0)) * speed;
            targetPos = pastPos + new Vector3(-6.5f, -6.5f);
        }
        else if (positionAlongRoom == 7)
        {
            rigidBody2D.velocity = Vector3.Normalize(new Vector3(-1, 1, 0)) * speed;
            targetPos = pastPos + new Vector3(-6.5f, 6.5f);
        }
        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            transform.position = targetPos;
            isMoving = false;
            positionAlongRoom += 2;
            positionAlongRoom %= 8;
            rigidBody2D.velocity = Vector3.zero;
            StartCoroutine(shootBlast(angleToShip));
            pastPos = transform.position;
        }
    }

    public void diagonalCCW()
    {
        if (positionAlongRoom == 1)
        {
            rigidBody2D.velocity = Vector3.Normalize(new Vector3(1, -1, 0)) * speed;
            targetPos = pastPos + new Vector3(6.5f, -6.5f);
        }
        else if (positionAlongRoom == 3)
        {
            rigidBody2D.velocity = Vector3.Normalize(new Vector3(-1, -1, 0)) * speed;
            targetPos = pastPos + new Vector3(-6.5f, -6.5f);
        }
        else if (positionAlongRoom == 5)
        {
            rigidBody2D.velocity = Vector3.Normalize(new Vector3(-1, 1, 0)) * speed;
            targetPos = pastPos + new Vector3(-6.5f, 6.5f);
        }
        else if (positionAlongRoom == 7)
        {
            rigidBody2D.velocity = Vector3.Normalize(new Vector3(1, 1, 0)) * speed;
            targetPos = pastPos + new Vector3(6.5f, 6.5f);
        }
        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            transform.position = targetPos;
            isMoving = false;
            positionAlongRoom -= 2;
            if (positionAlongRoom < 0)
            {
                positionAlongRoom = 8 + positionAlongRoom;
            }
            rigidBody2D.velocity = Vector3.zero;
            StartCoroutine(shootBlast(angleToShip));
            pastPos = transform.position;
        }
    }

    void pickSprite(float angleOrientation)
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            spriteRenderer.sprite = facingUpLeft;
            transform.localScale = new Vector3(-0.5f, 0.5f, 0);
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            spriteRenderer.sprite = facingUp;
            transform.localScale = new Vector3(0.5f, 0.5f, 0);
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            spriteRenderer.sprite = facingUpLeft;
            transform.localScale = new Vector3(0.5f, 0.5f, 0);
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(0.5f, 0.5f, 0);
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            spriteRenderer.sprite = facingDownLeft;
            transform.localScale = new Vector3(0.5f, 0.5f, 0);
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            spriteRenderer.sprite = facingDown;
            transform.localScale = new Vector3(0.5f, 0.5f, 0);
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            spriteRenderer.sprite = facingDownLeft;
            transform.localScale = new Vector3(-0.5f, 0.5f, 0);
        }
        else
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(-0.5f, 0.5f, 0);
        }
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        pastPos = transform.position;
	}

	void Update () {
        if (activatedRoom == true)
        {
            angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            if(isShooting == false)
                pickSprite(angleToShip);

            if(isMoving == true)
            {
                if (whatMovement == 1)
                {
                    straightCW();
                }
                else if (whatMovement == 2)
                {
                    straightCCW();
                }
                else if (whatMovement == 3)
                {
                    halfCW();
                }
                else if (whatMovement == 4)
                {
                    halfCCW();
                }
                else if (whatMovement == 5)
                {
                    diagonalCW();
                }
                else
                {
                    diagonalCCW();
                }
            }
            else
            {
                rigidBody2D.velocity = Vector3.zero;
            }

            spawnFoam();
        }
        pickRendererLayer();
    }

    int whatView()
    {
        if (spriteRenderer.sprite == facingLeft)
        {
            return 3;
        }
        else if (spriteRenderer.sprite == facingDownLeft)
        {
            return 2;
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            return 1;
        }
        else if (spriteRenderer.sprite == facingUpLeft)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && activatedRoom)
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
        GameObject deadSkeletonCannon = Instantiate(deadGolem, transform.position, Quaternion.identity);
        deadSkeletonCannon.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
        deadSkeletonCannon.GetComponent<DeadEnemyScript>().whatView = whatView();
        deadSkeletonCannon.transform.localScale = transform.localScale;
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
    }
}
