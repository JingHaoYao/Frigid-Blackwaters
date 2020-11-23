using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalCannon : Enemy {
    public Sprite facingUpRight, facingUp, facingLeft, facingDownLeft, facingDown;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    Animator animator;
    private bool isShooting;
    private float coolDownPeriod = 5, coolDownThreshold = 8;
    private float moveAngle = 0;
    GameObject playerShip;
    public GameObject cannonPlume, cannonRound;
    private float blastPeriod = 0;
    private float spriteFireAngle;
    public GameObject deadSkeleCannon;
    private float foamTimer = 0;
    public GameObject waterFoam;
    public float waitToFire = 0.5f;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    AStarPathfinding aStarPathfinding;

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

    void blastBack(float direction)
    {
        if (blastPeriod < 1f / 3f)
        {
            updateSpeed(8);
            rigidBody2D.velocity = speed * new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0);
            blastPeriod += Time.deltaTime;
        }
        else if(blastPeriod >= 1f/3f && blastPeriod < 1f)
        {
            updateSpeed((8 - 4 * (3 * (blastPeriod - 1f / 3f))));
            rigidBody2D.velocity = speed * new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0);
            blastPeriod += Time.deltaTime;
        }
        else if(blastPeriod >= 1f && blastPeriod <= 1.2f)
        {
            updateSpeed(0);
            rigidBody2D.velocity = Vector3.zero;
            blastPeriod += Time.deltaTime;
        }
        else
        {
            updateSpeed(2.5f);
            isShooting = false;
            blastPeriod = 0;
        }
    }

    IEnumerator fireCannon()
    {
        float angleFire = Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x);
        spriteFireAngle = (360 + angleFire * Mathf.Rad2Deg) % 360;
        pickSprite(spriteFireAngle);
        this.GetComponents<AudioSource>()[1].Play();
        if (spriteFireAngle > 15 && spriteFireAngle <= 75)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(.65f, 0.55f, 0), Quaternion.Euler(0, 0, 40)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 75 && spriteFireAngle <= 105)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0, 1.3f, 0), Quaternion.Euler(0, 0, 90)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 105 && spriteFireAngle <= 165)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-.65f, 0.55f, 0), Quaternion.Euler(0, 0, 140)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 165 && spriteFireAngle <= 195)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-0.9f, 0.25f, 0), Quaternion.Euler(0, 0, 180)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 195 && spriteFireAngle <= 255)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-0.6f, -0.3f, 0), Quaternion.Euler(0, 0, 225)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 255 && spriteFireAngle <= 285)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0, -0.3f, 0), Quaternion.Euler(0, 0, 270)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 285 && spriteFireAngle <= 345)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0.6f, -0.3f, 0), Quaternion.Euler(0, 0, 315)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0.9f, 0.25f, 0), Quaternion.Euler(0, 0, 0)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(8f / 36f);
    }

    void pickSprite(float angleOrientation)
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            spriteRenderer.sprite = facingUpRight;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            spriteRenderer.sprite = facingUp;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            spriteRenderer.sprite = facingUpRight;
            transform.localScale = new Vector3(-0.2f, 0.2f, 0);
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            spriteRenderer.sprite = facingDownLeft;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {

            spriteRenderer.sprite = facingDown;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            spriteRenderer.sprite = facingDownLeft;
            transform.localScale = new Vector3(-0.2f, 0.2f, 0);
        }
        else
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(-0.2f, 0.2f, 0);
        }
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        aStarPathfinding = GetComponent<AStarPathfinding>();
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

        if (Vector2.Distance(playerShip.transform.position, transform.position) > 5)
        {
            Vector3 targetPos = PlayerProperties.playerShipPosition;
            if (path.Count > 0)
            {
                AStarNode pathNode = path[0];
                targetPos = pathNode.nodePosition;
            }
            moveAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
            if (isShooting == false)
            {
                moveTowards(moveAngle);
            }
        }
        else
        {
            moveAngle = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            rigidBody2D.velocity = Vector3.zero;
            if (coolDownPeriod <= 0 && stopAttacking == false)
            {
                isShooting = true;
                StartCoroutine(fireCannon());
                coolDownPeriod = coolDownThreshold;
            }
        }
    }

    void Update () {
        this.aStarPathfinding.target = playerShip.transform.position;
        path = aStarPathfinding.seekPath;
        pickRendererLayer();
        findPositionShoot();

		if(isShooting == false)
        {
            pickSpritePeriod += Time.deltaTime;
            if (pickSpritePeriod >= 0.2f)
            {
                pickSprite(moveAngle);
                pickSpritePeriod = 0;
            }
        }

        if(isShooting == true)
        {
            blastBack(spriteFireAngle - 180);
        }
        spawnFoam();
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
        GameObject deadSkeletonCannon = Instantiate(deadSkeleCannon, transform.position, Quaternion.identity);
        deadSkeletonCannon.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
        deadSkeletonCannon.GetComponent<DeadEnemyScript>().whatView = whatView();
        deadSkeletonCannon.transform.localScale = transform.localScale;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    int whatView()
    {
        if (spriteRenderer.sprite == facingLeft)
        {
            return 1;
        }
        else if (spriteRenderer.sprite == facingDownLeft)
        {
            return 2;
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            return 3;
        }
        else if (spriteRenderer.sprite == facingUpRight)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }
}
