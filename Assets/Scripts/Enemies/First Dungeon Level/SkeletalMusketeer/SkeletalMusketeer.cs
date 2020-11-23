using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalMusketeer : Enemy {
    public Sprite facingLeft, facingDownLeft, facingDown, facingUpRight, facingUp;
    Rigidbody2D rigidBody2D;
    public GameObject weaponPlume, musketRound, deadSkeleMusketeer;
    SpriteRenderer spriteRenderer;
    GameObject playerShip;
    private bool isShooting;
    private float coolDownPeriod = 5;
    private float coolDownThreshold = 5;
    private float angleFire = 0;
    private float moveAngle = 0;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;

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

    IEnumerator fireMusket(float angleOrientation)
    {
        angleFire = Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x);
        float spriteFireAngle = (360 + angleFire * Mathf.Rad2Deg) % 360;
        pickSprite(spriteFireAngle);
        this.GetComponents<AudioSource>()[1].Play();
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            GameObject muskFlash = Instantiate(weaponPlume, transform.position + new Vector3(.65f, 0.55f, 0), Quaternion.Euler(0, 0, 40)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(musketRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalMusketRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            GameObject muskFlash = Instantiate(weaponPlume, transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 90)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(musketRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalMusketRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            GameObject muskFlash = Instantiate(weaponPlume, transform.position + new Vector3(-.65f, 0.55f, 0), Quaternion.Euler(0, 0, 140)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(musketRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalMusketRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            GameObject muskFlash = Instantiate(weaponPlume, transform.position + new Vector3(-0.9f, 0.25f, 0), Quaternion.Euler(0, 0, 180)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(musketRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalMusketRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            GameObject muskFlash = Instantiate(weaponPlume, transform.position + new Vector3(-0.7f, -0.15f, 0), Quaternion.Euler(0, 0, 220)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(musketRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalMusketRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            GameObject muskFlash = Instantiate(weaponPlume, transform.position + new Vector3(0, -0.3f, 0), Quaternion.Euler(0, 0, 270)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(musketRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalMusketRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            GameObject muskFlash = Instantiate(weaponPlume, transform.position + new Vector3(0.7f, -0.15f, 0), Quaternion.Euler(0, 0, 320)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(musketRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalMusketRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else
        {
            GameObject muskFlash = Instantiate(weaponPlume, transform.position + new Vector3(0.9f, 0.25f, 0), Quaternion.Euler(0, 0, 0)); //done
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(musketRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalMusketRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(8f / 36f);
        isShooting = false;
        
    }

    float cardinalizeDirections(float angle)
    {
        if(angle > 22.5f && angle <= 67.5f)
        {
            return 45;
        }
        else if(angle > 67.5f && angle <= 112.5f)
        {
            return 90;
        }
        else if(angle> 112.5f && angle <= 157.5f)
        {
            return 135;
        }
        else if(angle > 157.5f && angle <= 202.5f)
        {
            return 180;
        }
        else if(angle > 202.5f && angle <= 247.5f)
        {
            return 225;
        }
        else if(angle > 247.5 && angle < 292.5)
        {
            return 270;
        }
        else if(angle > 292.5 && angle < 337.5)
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

        if(Vector2.Distance(playerShip.transform.position, transform.position) > 6)
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
            float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            pickSprite(angleToShip);
            moveAngle = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            rigidBody2D.velocity = Vector3.zero;
            if (coolDownPeriod <= 0 && stopAttacking == false)
            {
                isShooting = true;
                StartCoroutine(fireMusket(moveAngle));
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

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
	}

	void Update () {
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
        else if(spriteRenderer.sprite == facingUpRight)
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
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void deathProcedure()
    {
        GameObject deadMusketeer = Instantiate(deadSkeleMusketeer, transform.position, Quaternion.identity);
        deadMusketeer.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
        deadMusketeer.GetComponent<DeadEnemyScript>().whatView = whatView();
        deadMusketeer.transform.localScale = transform.localScale;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
