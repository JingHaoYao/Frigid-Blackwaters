using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadMariner : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    GameObject playerShip;
    PlayerScript playerScript;
    int currView = 0;
    float angleToShip = 0;
    public GameObject anchor;
    float attackingPeriod = 6;
    GameObject anchorInstant;

    bool thrownBackAnim = true;
    public GameObject swordHitBox;
    bool swordAttacking = false;

    public GameObject cannonBall;
    public GameObject cannonPlume;

    int numberCannonsFired = 0;
    bool firingCannon = false;

    Vector3 targetTravel;

    Rigidbody2D rigidBody2D;

    public GameObject waterSplash;
    public GameObject waterFoam;
    float foamTimer = 0;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * 3 * 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    int pickView(float angle)
    {
        if (angle > 255 && angle <= 285)
        {
            return 1;
        }
        else if (angle > 285 && angle <= 360)
        {
            return 2;
        }
        else if (angle > 180 && angle <= 255)
        {
            return 3;
        }
        else if (angle > 75 && angle <= 105)
        {
            return 5;
        }
        else if (angle > 0 && angle <= 75)
        {
            return 6;
        }
        else
        {
            return 4;
        }
    }

    void pickIdleAnim(int view)
    {
        if (currView != view)
        {
            animator.SetTrigger("Idle" + view.ToString());
            currView = view;
        }
    }

    void pickThrowAnim(int view)
    {
        animator.SetTrigger("Throw" + view.ToString());
    }

    void pickSwordAnim(int view)
    {
        animator.SetTrigger("Sword" + view.ToString());
    }

    void throwAnchor(int view, float angle)
    {
        Vector3 returnPos;
        if (view == 1)
        {
            anchorInstant = Instantiate(anchor, transform.position + new Vector3(-0.9f, 1.049f), Quaternion.identity);
            returnPos = transform.position + new Vector3(-0.9f, 1.049f);
        }
        else if (view == 2)
        {
            anchorInstant = Instantiate(anchor, transform.position + new Vector3(.5f, 0.735f), Quaternion.identity);
            returnPos = transform.position + new Vector3(0.5f, 0.735f);
        }
        else if (view == 3)
        {
            anchorInstant = Instantiate(anchor, transform.position + new Vector3(-1.2f, 0.82f), Quaternion.identity);
            returnPos = transform.position + new Vector3(-1.2f, 0.82f);
        }
        else if (view == 4)
        {
            anchorInstant = Instantiate(anchor, transform.position + new Vector3(0f, 1.45f), Quaternion.identity);
            returnPos = transform.position + new Vector3(0, 1.45f);
        }
        else if (view == 5)
        {
            anchorInstant = Instantiate(anchor, transform.position + new Vector3(-1.05f, 2.04f), Quaternion.identity);
            returnPos = transform.position + new Vector3(-1.05f, 2.04f);
        }
        else
        {
            anchorInstant = Instantiate(anchor, transform.position + new Vector3(1.72f, 1.12f), Quaternion.identity);
            returnPos = transform.position + new Vector3(1.72f, 1.12f);
        }
        anchorInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        anchorInstant.GetComponent<UndeadMarinerAnchor>().returnPosition = returnPos;
        anchorInstant.GetComponent<UndeadMarinerAnchor>().attackingAngle = angle;
    }

    IEnumerator anchorAttack()
    {
        float attackingAngle = angleToShip;
        pickThrowAnim(pickView(attackingAngle));
        this.GetComponents<AudioSource>()[3].Play();
        yield return new WaitForSeconds(4f / 8f);
        throwAnchor(pickView(attackingAngle), attackingAngle);
    }

    IEnumerator swordAttack()
    {
        swordAttacking = true;
        int view = pickView(angleToShip);
        pickSwordAnim(view);
        this.GetComponents<AudioSource>()[5].Play();
        yield return new WaitForSeconds(3f / 8f);
        Collider2D[] hitBoxes = swordHitBox.GetComponents<PolygonCollider2D>();
        hitBoxes[view - 1].enabled = true;
        yield return new WaitForSeconds(2 / 8f);
        hitBoxes[view - 1].enabled = false;
        yield return new WaitForSeconds(1f / 8f);
        swordAttacking = false;
    }

    IEnumerator fireRightCannon(int whatView, float angle)
    {
        firingCannon = true;
        float orientationAngle = 0;
        Vector3 spawnLocation;
        if (whatView == 1)
        {
            spawnLocation = new Vector3(0.6f, 1.153f);
            orientationAngle = 270;
        }
        else if (whatView == 2)
        {
            spawnLocation = new Vector3(.776f, 1.463f);
            orientationAngle = 330;
        }
        else if (whatView == 3)
        {
            spawnLocation = new Vector3(-0.03f, 1.362f);
            orientationAngle = 210;
        }
        else if (whatView == 4)
        {
            spawnLocation = new Vector3(-1.2f, 1.791f);
            orientationAngle = 160;
        }
        else if (whatView == 5)
        {
            spawnLocation = new Vector3(-0.5f, 2.094f);
            orientationAngle = 90;
        }
        else
        {
            spawnLocation = new Vector3(0.45f, 1.847f);
            orientationAngle = 10;
        }
        GameObject plume = Instantiate(cannonPlume, transform.position + spawnLocation, Quaternion.Euler(0, 0, orientationAngle));
        this.GetComponents<AudioSource>()[0].Play();
        if (whatView > 3)
        {
            plume.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;
        }
        else
        {
            plume.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
        }
        yield return new WaitForSeconds(4f / 36f);
        GameObject cannonBallInstant = Instantiate(cannonBall, transform.position + spawnLocation, Quaternion.identity);
        cannonBallInstant.GetComponent<UndeadMarinerCannonBall>().angleTravel = angle * Mathf.Deg2Rad;
        cannonBallInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        yield return new WaitForSeconds(8f / 36f);
        firingCannon = false;
    }

    IEnumerator fireLeftCannon(int whatView, float angle)
    {
        firingCannon = true;
        Vector3 spawnLocation;
        float orientationAngle = 0;
        if (whatView == 1)
        {
            spawnLocation = new Vector3(-0.6f, 1.153f);
            orientationAngle = 270;
        }
        else if (whatView == 2)
        {
            spawnLocation = new Vector3(0.04f, 1.373f);
            orientationAngle = 330;
        }
        else if (whatView == 3)
        {
            spawnLocation = new Vector3(-0.8f, 1.44f);
            orientationAngle = 210;
        }
        else if (whatView == 4)
        {
            spawnLocation = new Vector3(-0.45f, 1.949f);
            orientationAngle = 160;
        }
        else if (whatView == 5)
        {
            spawnLocation = new Vector3(0.45f, 2.094f);
            orientationAngle = 90;
        }
        else
        {
            spawnLocation = new Vector3(1.131f, 1.819f);
            orientationAngle = 10;
        }
        GameObject plume = Instantiate(cannonPlume, transform.position + spawnLocation, Quaternion.Euler(0, 0, orientationAngle));
        this.GetComponents<AudioSource>()[0].Play();
        if (whatView > 3)
        {
            plume.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;
        }
        else
        {
            plume.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
        }
        yield return new WaitForSeconds(4f / 36f);
        GameObject cannonBallInstant = Instantiate(cannonBall, transform.position + spawnLocation, Quaternion.identity);
        cannonBallInstant.GetComponent<UndeadMarinerCannonBall>().angleTravel = angle * Mathf.Deg2Rad;
        cannonBallInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        yield return new WaitForSeconds(8f / 36f);
        firingCannon = false;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
        playerScript = playerShip.GetComponent<PlayerScript>();
        targetTravel = Camera.main.transform.position + new Vector3(Camera.main.transform.position.x - playerShip.transform.position.x, Camera.main.transform.position.y - playerShip.transform.position.y).normalized * 4.5f;
    }

    void Update()
    {
        angleToShip = (360 + (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg)) % 360;
        if (health > 0)
        {
            spawnFoam();
            if (attackingPeriod > 0)
            {
                if (anchorInstant)
                {
                    if (anchorInstant.GetComponent<UndeadMarinerAnchor>().returnToSender == true && thrownBackAnim == false)
                    {
                        thrownBackAnim = true;
                        currView = 0;
                        animator.SetTrigger("ThrowBack");
                        this.GetComponents<AudioSource>()[2].Play();
                    }
                }
                else if (thrownBackAnim == true)
                {
                    if (Vector2.Distance(transform.position, playerShip.transform.position) < 3)
                    {
                        if (swordAttacking == false)
                        {
                            StartCoroutine(swordAttack());
                        }
                    }
                    else
                    {
                        if (firingCannon == false)
                        {
                            attackingPeriod -= Time.deltaTime;
                            if (Vector2.Distance(transform.position, targetTravel) > 0.2f)
                            {
                                rigidBody2D.velocity = new Vector3(targetTravel.x - transform.position.x, targetTravel.y - transform.position.y).normalized * 3;
                            }
                            else
                            {
                                rigidBody2D.velocity = Vector3.zero;
                                targetTravel = Camera.main.transform.position + new Vector3(Camera.main.transform.position.x - playerShip.transform.position.x, Camera.main.transform.position.y - playerShip.transform.position.y).normalized * 4.5f;
                            }
                            pickIdleAnim(pickView(angleToShip));
                        }
                    }
                }
            }
            else
            {
                rigidBody2D.velocity = Vector3.zero;
                if (numberCannonsFired < 4)
                {
                    numberCannonsFired++;
                    attackingPeriod = .6f;
                    if (Random.Range(0, 2) == 1)
                    {
                        StartCoroutine(fireLeftCannon(pickView(angleToShip), angleToShip));
                    }
                    else
                    {
                        StartCoroutine(fireRightCannon(pickView(angleToShip), angleToShip));
                    }
                }
                else
                {
                    numberCannonsFired = 0;
                    attackingPeriod = 3;
                    thrownBackAnim = false;
                    StartCoroutine(anchorAttack());
                }
            }
        }
    }

    IEnumerator spawnWaterSplash()
    {
        yield return new WaitForSeconds(7f/12f);
        GameObject splash = Instantiate(waterSplash, transform.position, Quaternion.identity);
        splash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            this.GetComponents<AudioSource>()[4].Play();
            if (health <= 0)
            {
                rigidBody2D.velocity = Vector3.zero;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                addKills();
                MiscData.bossesDefeated.Add(nameID);
                animator.SetTrigger("Death");
                this.GetComponents<AudioSource>()[1].Play();
                Destroy(this.gameObject, 0.75f);
                StartCoroutine(spawnWaterSplash());
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