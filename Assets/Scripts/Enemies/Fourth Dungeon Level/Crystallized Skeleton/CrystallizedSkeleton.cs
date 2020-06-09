using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallizedSkeleton : Enemy
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
    [SerializeField] private AudioSource attackAudio;
    bool isAttacking = false;
    int whatView = 1;
    int mirror = 1;
    [SerializeField] InvisibilityEnemyController invisController;
    public List<CrystallizedSkeletonCrystal> spawnedCrystals;
    [SerializeField] GameObject largeCrystal;
    [SerializeField] GameObject smallCrystal;
    [SerializeField] LayerMask solidObstacleLayerMask;
    public bool underFog = false;

    float cageDuration = 0;

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Fogged Effect" || newStatus.name == "Fogged Effect(Clone)")
        {
            invisController.FogActivated();
            fadeOutCrystals();
            underFog = true;

            if (!isAttacking)
            {
                StartCoroutine(smashAttack());
            }
        }
    }

    public override void statusRemoved(EnemyStatusEffect removedStatus)
    {
        if (removedStatus.name == "Fogged Effect" || removedStatus.name == "Fogged Effect(Clone)")
        {
            underFog = false;
            invisController.FogDeActivated();
            fadeInCrystals();
        }
    }

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0 && invisController.isUnderLight)
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

    void moveTowards(float direction, float speedToTravel)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speedToTravel;
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
            whatView = 1;
            mirror = -1;
        }
        else if (angleOrientation >= 240 && angleOrientation < 300)
        {
            whatView = 2;
            mirror = 1;
        }
        else
        {
            whatView = 1;
            mirror = 1;
        }
    }

    private void Start()
    {
        animator.enabled = false;
        EnemyPool.addEnemy(this);
    }

    void Update()
    {
        spawnFoam();
        travelLocation();
    }

    IEnumerator smashAttack()
    {
        cageDuration = 4;
        animator.enabled = true;
        isAttacking = true;

        pickView(angleToShip());
        transform.localScale = new Vector3(4 * mirror, 4);
        animator.SetTrigger("Smash" + whatView);

        yield return new WaitForSeconds(5 / 12f);

        attackAudio.Play();
        for(int i = 0; i < 6; i++)
        {
            float angle = i * 60 * Mathf.Deg2Rad;
            Vector3 positionToSpawn = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 1f;
            if(!Physics2D.OverlapCircle(positionToSpawn, 0.5f, solidObstacleLayerMask))
            {
                GameObject crystalInstant = Instantiate(largeCrystal, positionToSpawn, Quaternion.identity);
                if(Random.Range(0, 2) == 1)
                {
                    crystalInstant.transform.localScale = new Vector3(-5, 5, 0);
                }
                CrystallizedSkeletonCrystal crystalScript = crystalInstant.GetComponent<CrystallizedSkeletonCrystal>();
                crystalScript.crystallizedSkeleton = this;
                spawnedCrystals.Add(crystalScript);
                crystalInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }

        }

        yield return new WaitForSeconds(2 / 12f);

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45 * Mathf.Deg2Rad;
            Vector3 positionToSpawn = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 1.7f;
            if (!Physics2D.OverlapCircle(positionToSpawn, 0.5f, solidObstacleLayerMask))
            {
                GameObject crystalInstant = Instantiate(smallCrystal, positionToSpawn, Quaternion.identity);
                if (Random.Range(0, 2) == 1)
                {
                    crystalInstant.transform.localScale = new Vector3(-5, 5, 0);
                }
                CrystallizedSkeletonCrystal crystalScript = crystalInstant.GetComponent<CrystallizedSkeletonCrystal>();
                crystalScript.crystallizedSkeleton = this;
                spawnedCrystals.Add(crystalScript);
                crystalInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }

        }

        yield return new WaitForSeconds(3 / 12f);

        animator.enabled = false;
        while(cageDuration > 0)
        {
            cageDuration -= Time.deltaTime;
            pickView(angleToShip());
            spriteRenderer.sprite = viewSprites[whatView - 1];
            transform.localScale = new Vector3(4 * mirror, 4);
            yield return null;
        }

        shatterCrystals();
        isAttacking = false;
    }

    void fadeOutCrystals()
    {
        foreach(CrystallizedSkeletonCrystal crystal in spawnedCrystals)
        {
            crystal.fadeOut();
        }
    }

    void fadeInCrystals()
    {
        foreach(CrystallizedSkeletonCrystal crystal in spawnedCrystals)
        {
            crystal.fadeIn();
        }
    }

    void shatterCrystals()
    {
        while(spawnedCrystals.Count > 0)
        {
            spawnedCrystals[0].shatter();
        }
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


        if (path != null && path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 2.5f && isAttacking == false)
        {
            moveTowards(travelAngle, this.speed);

            pickSpritePeriod += Time.deltaTime;
            if (pickSpritePeriod >= 0.2f)
            {
                pickView(travelAngle);
                pickSpritePeriod = 0;
                spriteRenderer.sprite = viewSprites[whatView - 1];
                transform.localScale = new Vector3(4 * mirror, 4);
            }
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            if (isAttacking == false)
            {
                StartCoroutine(smashAttack());
            }
        }

        if (!invisController.isUnderLight)
        {
            cageDuration = 2;
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
        invisController.hideRendererAfterHit();
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
        shatterCrystals();
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
