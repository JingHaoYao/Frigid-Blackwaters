using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicFish : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private float travelAngle;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private AudioSource bigEruptionAudio;
    bool isAttacking = false;
    int whatView = 1;
    int mirror = 1;
    [SerializeField] GameObject arcingProjectile;
    [SerializeField] Sprite[] viewSprites;
    int prevView = 0;
    private float attackPeriod = 0;
    Camera mainCamera;
    bool hasErupted = false;

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
        transform.localScale = new Vector3(mirror * 3, 3);
    }

    void pickIdleAnimation()
    {
        pickView(angleToShip());
        spriteRenderer.sprite = viewSprites[whatView - 1];
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

    IEnumerator bigEruption()
    {
        while(isAttacking == true)
        {
            yield return null;
        }

        isAttacking = true;
        pickView(angleToShip());
        animator.enabled = true;
        animator.SetTrigger("BigEruption");
        
        yield return new WaitForSeconds(5 / 12f);
        bigEruptionAudio.Play();

        if(stopAttacking == false)
        {
            for(int i = 0; i <  8; i++)
            {
                GameObject arcingProjectileInstant = Instantiate(arcingProjectile, transform.position + Vector3.up * 1.5f, Quaternion.identity);
                Thornball thornBallInstant = arcingProjectileInstant.GetComponent<Thornball>();
                thornBallInstant.targetLocation = PickProjectileLocation();
                arcingProjectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }

        yield return new WaitForSeconds(5 / 12f);
        pickView(angleToShip());
        animator.enabled = false;
        isAttacking = false;
    }

    Vector3 PickProjectileLocation()
    {
        return mainCamera.transform.position + new Vector3(Random.Range(-6.0f, 6.0f), Random.Range(-6.0f, 6.0f));
    }

    Vector3 PickRandomPositionAroundShip()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        Vector3 potentialPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(0.75f, 1.5f);
        return new Vector3(Mathf.Clamp(potentialPosition.x, mainCamera.transform.position.x - 8f, mainCamera.transform.position.x + 8f), Mathf.Clamp(potentialPosition.y, mainCamera.transform.position.y - 8f, mainCamera.transform.position.y + 8f));
    }

    IEnumerator smallEruption()
    {
        isAttacking = true;
        pickView(angleToShip());
        animator.enabled = true;
        animator.SetTrigger("Erupt" + whatView);
        yield return new WaitForSeconds(4 / 12f);
        attackAudio.Play();
        if (stopAttacking == false)
        {
            GameObject arcingProjectileInstant = Instantiate(arcingProjectile, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            arcingProjectileInstant.GetComponent<Thornball>().targetLocation = PlayerProperties.playerShipPosition;
            arcingProjectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;

            for(int i = 0; i < 3; i++)
            {
                arcingProjectileInstant = Instantiate(arcingProjectile, transform.position + Vector3.up * 1.5f, Quaternion.identity);
                arcingProjectileInstant.GetComponent<Thornball>().targetLocation = PickRandomPositionAroundShip();
                arcingProjectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        yield return new WaitForSeconds(5 / 12f);
        pickView(angleToShip());
        animator.enabled = false;
        isAttacking = false;
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = PlayerProperties.playerShipPosition;
        Vector3 targetPos = PlayerProperties.playerShipPosition;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(PlayerProperties.playerShipPosition, transform.position) > 3 && isAttacking == false)
        {
            moveTowards(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }

        if (attackPeriod < 3)
        {
            attackPeriod += Time.deltaTime;
        }
        else
        {
            StartCoroutine(smallEruption());
            attackPeriod = 0;
        }

        if (isAttacking == false)
        {
            pickIdleAnimation();
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

        if((float)health / maxHealth <= 0.5f && hasErupted == false)
        {
            hasErupted = true;
            StartCoroutine(bigEruption());
        }
        damageAudio.Play();
    }
}
