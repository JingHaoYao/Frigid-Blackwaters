using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditShip : MonoBehaviour {
    public Sprite facingUpLeft, facingUp, facingLeft, facingDownLeft, facingDown;
    public Collider2D upHitBox, downHitBox, leftHitBox, upLeftHitBox, downLeftHitBox;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    float travelSpeed = 2.5f;
    public bool cw = false;
    public GameObject cannonPlume, cannonRound;
    GameObject playerShip;
    float angleToShip;
    float orientationAngle = 0;
    float firePeriod = 0;
    public int health = 5;
    public GameObject deadShip;
    private float foamTimer = 0;
    public GameObject waterFoam;

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

    void circlePlayerShip()
    {
        float circleAngle;
        if (cw == true)
        {
            circleAngle = (270 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
        else
        {
            circleAngle = (450 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
        rigidBody2D.velocity = new Vector3(Mathf.Cos(circleAngle * Mathf.Deg2Rad) + Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(circleAngle * Mathf.Deg2Rad) + Mathf.Sin(angleToShip * Mathf.Deg2Rad), 0) * travelSpeed;
        orientationAngle = (Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg + 360) % 360;
        pickSprite(orientationAngle);

        if(rigidBody2D.velocity.magnitude > 1f)
        {
            if (this.GetComponents<AudioSource>()[1].isPlaying == false)
            {
                this.GetComponents<AudioSource>()[1].Play();
            }
        }
        else
        {
            if (this.GetComponents<AudioSource>()[1].isPlaying == true)
            {
                this.GetComponents<AudioSource>()[1].Stop();
            }
        }
    }

    void pickHitBox()
    {
        if(spriteRenderer.sprite == facingUpLeft)
        {
            upLeftHitBox.enabled = true;
            upHitBox.enabled = false;
            downHitBox.enabled = false;
            leftHitBox.enabled = false;
            downLeftHitBox.enabled = false;
        }
        else if(spriteRenderer.sprite == facingDownLeft)
        {
            upLeftHitBox.enabled = false;
            upHitBox.enabled = false;
            downHitBox.enabled = false;
            leftHitBox.enabled = false;
            downLeftHitBox.enabled = true;
        }
        else if(spriteRenderer.sprite == facingLeft)
        {
            upLeftHitBox.enabled = false;
            upHitBox.enabled = false;
            downHitBox.enabled = false;
            leftHitBox.enabled = true;
            downLeftHitBox.enabled = false;
        }
        else if(spriteRenderer.sprite == facingUp)
        {
            upLeftHitBox.enabled = false;
            upHitBox.enabled = true;
            downHitBox.enabled = false;
            leftHitBox.enabled = false;
            downLeftHitBox.enabled = false;
        }
        else
        {
            upLeftHitBox.enabled = false;
            upHitBox.enabled = false;
            downHitBox.enabled = true;
            leftHitBox.enabled = false;
            downLeftHitBox.enabled = false;
        }
    }

    void pickSprite(float angleOrientation)
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            spriteRenderer.sprite = facingUpLeft;
            transform.localScale = new Vector3(-0.3f, 0.3f, 0);
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            spriteRenderer.sprite = facingUp;
            if (cw == true)
            {
                transform.localScale = new Vector3(-0.3f, 0.3f, 0);
            }
            else
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 0);
            }
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            spriteRenderer.sprite = facingUpLeft;
            transform.localScale = new Vector3(0.3f, 0.3f, 0);
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(0.3f, 0.3f, 0);
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            spriteRenderer.sprite = facingDownLeft;
            transform.localScale = new Vector3(0.3f, 0.3f, 0);
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {

            spriteRenderer.sprite = facingDown;
            if(cw == true)
            {
                transform.localScale = new Vector3(-0.3f, 0.3f, 0);
            }
            else
            {
                transform.localScale = new Vector3(0.3f, 0.3f, 0);
            }
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            spriteRenderer.sprite = facingDownLeft;
            transform.localScale = new Vector3(-0.3f, 0.3f, 0);
        }
        else
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(-0.3f, 0.3f, 0);
        }
    }

    IEnumerator fireCannonCW()
    {
        float angleFire = Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x);
        float spriteFireAngle = (360 + angleFire * Mathf.Rad2Deg) % 360;
        pickSprite(((spriteFireAngle - 90) + 360) % 360);
        this.GetComponents<AudioSource>()[0].Play();

        if (spriteFireAngle > 15 && spriteFireAngle <= 75)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0.79f, 0.79f, 0), Quaternion.Euler(0, 0, 30)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 75 && spriteFireAngle <= 105)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0.1f, 1.172f, 0), Quaternion.Euler(0, 0, 90)); // done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 105 && spriteFireAngle <= 165)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-0.93f, 0.62f, 0), Quaternion.Euler(0, 0, 150)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 165 && spriteFireAngle <= 195)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-0.91f, 0.116f, 0), Quaternion.Euler(0, 0, 180)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 195 && spriteFireAngle <= 255)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-0.94f, -0.184f, 0), Quaternion.Euler(0, 0, 210)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 255 && spriteFireAngle <= 285)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-0.1f, -0.5f, 0), Quaternion.Euler(0, 0, 270)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 285 && spriteFireAngle <= 345)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(1.08f, -0.135f, 0), Quaternion.Euler(0, 0, 330));//done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0.92f, 0.1f, 0), Quaternion.Euler(0, 0, 0)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(8f / 36f);
    }

    IEnumerator fireCannonCCW()
    {
        float angleFire = Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x);
        float spriteFireAngle = (360 + angleFire * Mathf.Rad2Deg) % 360;
        pickSprite(((spriteFireAngle + 90) + 360) % 360);
        this.GetComponents<AudioSource>()[0].Play();

        if (spriteFireAngle > 15 && spriteFireAngle <= 75)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0.76f, 0.74f, 0), Quaternion.Euler(0, 0, 30)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 75 && spriteFireAngle <= 105)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-0.22f, 1.28f, 0), Quaternion.Euler(0, 0, 90)); // done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 105 && spriteFireAngle <= 165)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-0.93f, 0.6f, 0), Quaternion.Euler(0, 0, 150)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 165 && spriteFireAngle <= 195)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-0.88f, 0.16f, 0), Quaternion.Euler(0, 0, 180)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 195 && spriteFireAngle <= 255)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(-1f, -0.22f, 0), Quaternion.Euler(0, 0, 210)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 255 && spriteFireAngle <= 285)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0.16f, -0.5f, 0), Quaternion.Euler(0, 0, 270)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (spriteFireAngle > 285 && spriteFireAngle <= 345)
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(1.06f, -0.12f, 0), Quaternion.Euler(0, 0, 330));//done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else
        {
            GameObject muskFlash = Instantiate(cannonPlume, transform.position + new Vector3(0.93f, 0.29f, 0), Quaternion.Euler(0, 0, 0)); //done
            muskFlash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
            yield return new WaitForSeconds(4f / 36f);
            GameObject instantiatedRound = Instantiate(cannonRound, muskFlash.transform.position, Quaternion.identity);
            instantiatedRound.GetComponent<SkeletalCannonRound>().angleTravel = angleFire;
            instantiatedRound.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(8f / 36f);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
	}

	void Update () {
        angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        pickRendererLayer();
        pickHitBox();
        spawnFoam();
        if(Vector2.Distance(playerShip.transform.position, transform.position) > 5)
        {
            circlePlayerShip();
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            firePeriod += Time.deltaTime;
            if(firePeriod > 3)
            {
                if(cw == false)
                {
                    StartCoroutine(fireCannonCCW());
                }
                else
                {
                    StartCoroutine(fireCannonCW());
                }
                firePeriod = 0;
            }
        }
	}

    int whatView()
    {
        if (spriteRenderer.sprite == facingLeft)
        {
            return 3;
        }
        else if (spriteRenderer.sprite == facingDownLeft)
        {
            return 4;
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            return 5;
        }
        else if (spriteRenderer.sprite == facingUpLeft)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
            if (health <= 0)
            {
                GameObject deadSkeletonCannon = Instantiate(deadShip, transform.position, Quaternion.identity);
                deadSkeletonCannon.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
                deadSkeletonCannon.GetComponent<DeadEnemyScript>().whatView = whatView();
                deadSkeletonCannon.transform.localScale = transform.localScale;
                Destroy(this.gameObject);
            }
            else
            {
                StartCoroutine(hitFrame());
            }
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
