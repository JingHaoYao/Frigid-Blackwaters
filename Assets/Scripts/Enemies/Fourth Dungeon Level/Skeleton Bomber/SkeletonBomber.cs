using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBomber : Enemy
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
    private Vector3 randomPos;
    int whatView = 1;
    int mirror = 1;
    public GameObject flyingBomb;
    [SerializeField] InvisibilityEnemyController invisController;
    List<SkeletonBomberFloatingProjectile> bombList = new List<SkeletonBomberFloatingProjectile>();
    Camera mainCamera;
    [SerializeField] LayerMask bombLocationLayerMask;
    public bool bomberUnderLight = false;

    IEnumerator activateBombs()
    {
        yield return new WaitForSeconds(1);
        foreach(SkeletonBomberFloatingProjectile bomb in bombList)
        {
            bomb.activateBomb();
        }
        bombList.Clear();
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Fogged Effect" || newStatus.name == "Fogged Effect(Clone)")
        {
            bomberUnderLight = true;
            invisController.FogActivated();
            StartCoroutine(activateBombs());
            fadeOutBombs();
        }
    }

    public override void statusRemoved(EnemyStatusEffect removedStatus)
    {
        if (removedStatus.name == "Fogged Effect" || removedStatus.name == "Fogged Effect(Clone)")
        {
            bomberUnderLight = false;
            invisController.FogDeActivated();
            fadeInBombs();
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


    IEnumerator throwBomb()
    {
        animator.enabled = true;
        isAttacking = true;
        Vector3 target = pickTargetPosition();
        float angle = (Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;
        pickView(angle);
        transform.localScale = new Vector3(4 * mirror, 4);
        animator.SetTrigger("Throw" + whatView);
        yield return new WaitForSeconds(8 / 12f);
        attackAudio.Play();
        if (stopAttacking == false)
        {
            GameObject bombProjectileInstant = Instantiate(flyingBomb, transform.position, Quaternion.identity);
            bombProjectileInstant.GetComponent<SkeletonBomberFlyingBombProjectile>().Initialize(this, target);
        }
        yield return new WaitForSeconds(1 / 12f);
        isAttacking = false;
        animator.enabled = false;
        randomPos = pickRandPos();
    }

    Vector3 pickTargetPosition()
    {
        Vector3 randomPosition = new Vector3(mainCamera.transform.position.x + Random.Range(-8.0f, 8.0f), mainCamera.transform.position.y + Random.Range(-8.0f, 8.0f));
        while(Physics2D.OverlapCircle(randomPosition, 0.4f, bombLocationLayerMask) || isMineTooCloseToOtherMines(randomPosition))
        {
            randomPosition = new Vector3(mainCamera.transform.position.x + Random.Range(-8.0f, 8.0f), mainCamera.transform.position.y + Random.Range(-8.0f, 8.0f));
        }
        return randomPosition;
    }

    bool isMineTooCloseToOtherMines(Vector3 pos)
    {
        foreach(SkeletonBomberFloatingProjectile bomb in bombList)
        {
            if(Vector2.Distance(bomb.transform.position, pos) < 3)
            {
                return true;
            }
        }
        return false;
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

    private void Start()
    {
        EnemyPool.addEnemy(this);
        randomPos = pickRandPos();
        animator.enabled = false;
        mainCamera = Camera.main;
    }

    void Update()
    {
        spawnFoam();
        travelLocation();
    }

    void fadeOutBombs()
    {
        foreach (SkeletonBomberFloatingProjectile bomb_ in bombList)
        {
            LeanTween.alpha(bomb_.gameObject, 0, 0.5f);
        }
    }

    void fadeInBombs()
    {
        foreach (SkeletonBomberFloatingProjectile bomb_ in bombList)
        {
            LeanTween.alpha(bomb_.gameObject, 1, 0.5f);
        }
    }

    public void removeBomb(SkeletonBomberFloatingProjectile bomb_)
    {
        bombList.Remove(bomb_);
    }

    public void addBomb(SkeletonBomberFloatingProjectile bomb_)
    {
        bombList.Add(bomb_);
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = randomPos;
        Vector3 targetPos = Vector3.zero;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (path != null && path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f)
        {
            moveTowards(travelAngle);

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
            if (isAttacking == false && Vector2.Distance(transform.position, randomPos) < 1f && stopAttacking == false)
            {
                if (invisController.isUnderLight && bombList.Count < 4)
                {
                    StartCoroutine(throwBomb());
                }
                else
                {
                    randomPos = pickRandPos();
                }
            }
        }
    }

    Vector3 pickRandPos()
    {
        float randX;
        float randY;
        if (Random.Range(0, 2) == 1)
        {
            randX = transform.position.x + Random.Range(4.0f, 7.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(4.0f, 7.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-7.0f, -4.0f);
            }
        }
        else
        {
            randX = transform.position.x + Random.Range(-7.0f, -4.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(4.0f, 7.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-7.0f, -4.0f);
            }
        }

        Vector3 randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        while (Physics2D.OverlapCircle(randPos, .5f) || Vector2.Distance(randPos, transform.position) < 2)
        {
            if (Random.Range(0, 2) == 1)
            {
                randX = transform.position.x + Random.Range(4.0f, 7.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(4.0f, 7.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-7.0f, -4.0f);
                }
            }
            else
            {
                randX = transform.position.x + Random.Range(-7.0f, -4.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(4.0f, 7.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-7.0f, -4.0f);
                }
            }
            randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
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
        foreach (SkeletonBomberFloatingProjectile bomb_ in bombList)
        {
            bomb_.fadeAway();
        }
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
