using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalShield : Enemy {
    public Sprite facingUp, facingDown, facingLeft, facingRight;
    public GameObject upShield, downShield, leftShield, rightShield;
    public GameObject upDamageHitBox, downDamageHitBox, leftDamageHitBox, rightDamageHitBox;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    public float travelSpeed = 1;
    private float foamTimer = 0;
    public GameObject waterFoam;
    public GameObject protectedEnemy;
    private float angleToShip = 0;
    GameObject playerShip;
    Vector3 targetPos;
    private bool noMoreEnemies = false;
    public GameObject deadShield;
    public bool actualHit = false;
    List<AStarNode> path;

    void pickRangedEnemy()
    {
        if (protectedEnemy == null)
        {
            GameObject[] rangeEnemyList = GameObject.FindGameObjectsWithTag("RangedEnemy");
            if(rangeEnemyList.Length == 0)
            {
                noMoreEnemies = true;
            }
            protectedEnemy = rangeEnemyList[Random.Range(0, rangeEnemyList.Length)];
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

    void protectEnemy()
    {
        path = GetComponent<AStarPathfinding>().seekPath;
        AStarNode pathNode = path[0];
        Vector3 targetPospath = pathNode.nodePosition;
        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPospath.y - (transform.position.y + 0.4f), targetPospath.x - transform.position.x) * Mathf.Rad2Deg) % 360);
        float angleProtect = Mathf.Atan2(playerShip.transform.position.y - protectedEnemy.transform.position.y, playerShip.transform.position.x - protectedEnemy.transform.position.x);
        targetPos = protectedEnemy.transform.position + new Vector3(Mathf.Cos(angleProtect), Mathf.Sin(angleProtect), 0) * 2;
        if(Vector2.Distance(transform.position, targetPos) <= 0.1f)
        {
            pickSprite(angleToShip);
            rigidBody2D.velocity = Vector3.zero;
        }
        else
        {
            if (Vector2.Distance(transform.position, protectedEnemy.transform.position) > 3)
            {
                this.GetComponent<AStarPathfinding>().target = protectedEnemy.transform.position;
                pickSprite(travelAngle);
                moveTowards(travelAngle);
            }
            else
            {
                float angleTravel = (360 + Mathf.Atan2(targetPos.y - transform.position.y, targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                pickSprite(angleToShip);
                moveTowards(angleTravel);
            }
        }
    }

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * rigidBody2D.velocity.magnitude / 3f)
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
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
    }

    void pickShield()
    {
        if (spriteRenderer.sprite == facingUp)
        {
            upShield.SetActive(true);
            downShield.SetActive(false);
            leftShield.SetActive(false);
            rightShield.SetActive(false);
            upDamageHitBox.SetActive(true);
            downDamageHitBox.SetActive(false);
            leftDamageHitBox.SetActive(false);
            rightDamageHitBox.SetActive(false);
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            upShield.SetActive(false);
            downShield.SetActive(true);
            leftShield.SetActive(false);
            rightShield.SetActive(false);
            upDamageHitBox.SetActive(false);
            downDamageHitBox.SetActive(true);
            leftDamageHitBox.SetActive(false);
            rightDamageHitBox.SetActive(false);
        }
        else if(spriteRenderer.sprite == facingLeft)
        {
            upShield.SetActive(false);
            downShield.SetActive(false);
            leftShield.SetActive(true);
            rightShield.SetActive(false);
            upDamageHitBox.SetActive(false);
            downDamageHitBox.SetActive(false);
            leftDamageHitBox.SetActive(true);
            rightDamageHitBox.SetActive(false);
        }
        else
        {
            upShield.SetActive(false);
            downShield.SetActive(false);
            leftShield.SetActive(false);
            rightShield.SetActive(true);
            upDamageHitBox.SetActive(false);
            downDamageHitBox.SetActive(false);
            leftDamageHitBox.SetActive(false);
            rightDamageHitBox.SetActive(true);
        }
    }

    void pickSprite(float direction)
    {
        if (direction > 75 && direction < 105)
        {
            spriteRenderer.sprite = facingUp;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
        else if (direction < 285 && direction > 265)
        {
            spriteRenderer.sprite = facingDown;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
        else if (direction >= 285 && direction <= 360)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(-0.2f, 0.2f, 0);
        }
        else if (direction >= 180 && direction <= 265)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
        else if (direction < 180 && direction >= 105)
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(-0.2f, 0.2f, 0);
        }
        else
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
        pickRangedEnemy();
    }

	void Update () {
        pickRendererLayer();
        pickShield();
        if (noMoreEnemies == false)
        {
            pickRangedEnemy();
            spawnFoam();
            angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            protectEnemy();
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }

        if (actualHit == true)
        {
            if (health <= 0)
            {
                GameObject deadPirate = Instantiate(deadShield, transform.position, Quaternion.identity);
                deadPirate.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
                deadPirate.GetComponent<DeadEnemyScript>().whatView = whatView();
                deadPirate.transform.localScale = transform.localScale;
                addKills();
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(this.gameObject);
            }
            else
            {
                StartCoroutine(hitFrame());
            }
            actualHit = false;
        }
    }

    int whatView()
    {
        if (spriteRenderer.sprite == facingUp)
        {
            return 4;
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            return 2;
        }
        else if (spriteRenderer.sprite == facingLeft)
        {
            return 1;
        }
        else
        {
            return 3;
        }
    }

    IEnumerator hitFrame()
    {
        this.GetComponents<AudioSource>()[0].Play();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
