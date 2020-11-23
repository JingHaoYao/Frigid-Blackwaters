using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAssassin : Enemy
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
    bool attacking = false;
    [SerializeField] InvisibilityEnemyController invisController;
    [SerializeField] float withinRangeRadius;
    [SerializeField] GameObject strikeAttack, teleportEffect;
    Camera mainCamera;

    int whatView = 1;
    int mirror = 1;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Fogged Effect" || newStatus.name == "Fogged Effect(Clone)")
        {
            invisController.FogActivated();
        }
    }

    public override void statusRemoved(EnemyStatusEffect removedStatus)
    {
        if (removedStatus.name == "Fogged Effect" || removedStatus.name == "Fogged Effect(Clone)")
        {
            invisController.FogDeActivated();
        }
    }

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0 && this.invisController.isUnderLight == true)
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

    void Update()
    {
        pickRendererLayer();
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = PlayerProperties.playerShipPosition;
        Vector3 targetPos = PlayerProperties.playerShipPosition;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        pickSpritePeriod += Time.deltaTime;

        if (attacking == false)
        {
            moveTowards(travelAngle);
            animator.enabled = false;
            if (pickSpritePeriod >= 0.2f)
            {
                spriteRenderer.sprite = viewSprites[whatView - 1];
                transform.localScale = new Vector3(4 * mirror, 4);
                pickView(angleToShip());
                pickSpritePeriod = 0;
            }

            if(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < withinRangeRadius)
            {
                StartCoroutine(sliceAndTeleport());
            }
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }

        spawnFoam();
    }

    float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    IEnumerator sliceAndTeleport()
    {
        pickView(angleToShip());
        animator.enabled = true;
        attacking = true;
        animator.SetTrigger("Attack" + whatView);
        yield return new WaitForSeconds(5 / 12f);
        if(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) <= withinRangeRadius)
        {
            GameObject instant = Instantiate(strikeAttack, PlayerProperties.playerShipPosition, Quaternion.identity);
            instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            Vector3 randomPosition = pickRandomPosition();
            Instantiate(teleportEffect, randomPosition + Vector3.up * 0.5f, Quaternion.identity);
            transform.position = randomPosition;
            animator.enabled = false;
            pickView(angleToShip());
            spriteRenderer.sprite = viewSprites[whatView - 1];
            transform.localScale = new Vector3(4 * mirror, 4);
            yield return new WaitForSeconds(0.5f);
            attacking = false;
        }
        else
        {
            animator.enabled = false;
            attacking = false;
        }
    }

    Vector3 pickRandomPosition()
    {
        Vector3 randPos = new Vector3(mainCamera.transform.position.x + Random.Range(-8.0f, 8.0f), mainCamera.transform.position.y + Random.Range(-8.0f, 8.0f));
        while(Physics2D.OverlapCircle(randPos, 0.4f, 12) || Vector2.Distance(randPos, PlayerProperties.playerShipPosition) < 4) {
            randPos = new Vector3(mainCamera.transform.position.x + Random.Range(-8.0f, 8.0f), mainCamera.transform.position.y + Random.Range(-8.0f, 8.0f));
        }
        return randPos;
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
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
