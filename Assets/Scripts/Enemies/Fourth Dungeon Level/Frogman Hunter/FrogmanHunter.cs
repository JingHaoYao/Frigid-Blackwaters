using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanHunter : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    [SerializeField] Sprite[] equippedViewSprites;
    [SerializeField] Sprite[] unEquippedViewSprites;
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
    private bool equipped = true;
    int whatView = 1;
    int mirror = 1;
    public GameObject spearProjectile;
    [SerializeField] InvisibilityEnemyController invisController;
    private FrogmanSupplier frogmanSupplier;
    bool supplierDefeated = false;
    private Vector3 targetPosition;

    // This enemy needs to be added to enemy pool
    public void Initialize(FrogmanSupplier supplier)
    {
        this.frogmanSupplier = supplier;
    }

    public void SupplierDefeated()
    {
        supplierDefeated = true;
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

    void spawnProjectiles(float angleAttack)
    {
        GameObject instant = Instantiate(spearProjectile, transform.position + new Vector3(Mathf.Cos(angleAttack * Mathf.Deg2Rad), Mathf.Sin(angleAttack * Mathf.Deg2Rad)) * 0.5f, Quaternion.identity);
        if (invisController.isUnderLight)
        {
            instant.GetComponent<FrogmanHunterSpearProjectile>().Initialize(angleAttack, this.gameObject, false);
        }
        else
        {
            instant.GetComponent<FrogmanHunterSpearProjectile>().Initialize(angleAttack, this.gameObject, true);
        }
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    Vector3 pickRandPos()
    {
        float randX;
        float randY;
        if (Random.Range(0, 2) == 1)
        {
            randX = transform.position.x + Random.Range(5.0f, 6.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 6.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-6.0f, -5.0f);
            }
        }
        else
        {
            randX = transform.position.x + Random.Range(-6.0f, -5.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 6.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-6.0f, -5.0f);
            }
        }

        Vector3 randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        while (Physics2D.OverlapCircle(randPos, .5f) || Vector2.Distance(randPos, transform.position) < 2)
        {
            if (Random.Range(0, 2) == 1)
            {
                randX = transform.position.x + Random.Range(5.0f, 6.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 6.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-6.0f, -5.0f);
                }
            }
            else
            {
                randX = transform.position.x + Random.Range(-6.0f, -5.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 6.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-6.0f, -5.0f);
                }
            }
            randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        }
        return randPos;
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
    }

    private void Start()
    {
        animator.enabled = false;
        targetPosition = pickRandPos();
    }

    void Update()
    {
        spawnFoam();
        travelLocation();
    }

    IEnumerator launchProjectiles()
    {
        animator.enabled = true;
        isAttacking = true;
        pickView(angleToShip());
        float angleAttack = angleToShip();
        Vector3 targetPosition = PlayerProperties.playerShipPosition;
        transform.localScale = new Vector3(3 * mirror, 3);
        animator.SetTrigger("Throw" + whatView);
        yield return new WaitForSeconds(7 / 12f);
        attackAudio.Play();
        if (stopAttacking == false)
            spawnProjectiles(angleAttack);
        yield return new WaitForSeconds(3 / 12f);
        equipped = false;
        isAttacking = false;
        animator.enabled = false;
        pickView(angleToShip());
        pickSprite(whatView);
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = equipped ? targetPosition : (supplierDefeated ? Vector3.zero : frogmanSupplier.transform.position);
        Vector3 targetPos = Vector3.zero;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if(!equipped)
        {
            if (supplierDefeated == false)
            {
                if (Vector2.Distance(transform.position, frogmanSupplier.transform.position) < 2.5f)
                {
                    equipped = true;
                    targetPosition = pickRandPos();
                }
            }
        }

        if (
            path.Count > 0 
            && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f 
            && (Vector2.Distance(transform.position, targetPosition) > 1f || !equipped) 
            && isAttacking == false
            && supplierDefeated == false)
        {
            moveTowards(travelAngle);

            pickSpritePeriod += Time.deltaTime;
            if (pickSpritePeriod >= 0.2f)
            {
                pickView(travelAngle);
                pickSpritePeriod = 0;
                pickSprite(whatView);
                transform.localScale = new Vector3(3 * mirror, 3);
            }
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;

            if (equipped == true)
            {
                if (isAttacking == false)
                {
                    StartCoroutine(launchProjectiles());
                }
            }
            else
            {
                if (pickSpritePeriod >= 0.2f)
                {
                    pickView(angleToShip());
                    pickSpritePeriod = 0;
                    pickSprite(whatView);
                    transform.localScale = new Vector3(3 * mirror, 3);
                }
            }
        }
    }

    void pickSprite(int whatView)
    {
        if (equipped)
        {
            spriteRenderer.sprite = equippedViewSprites[whatView - 1];
        }
        else
        {
            spriteRenderer.sprite = unEquippedViewSprites[whatView - 1];
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
        frogmanSupplier.frogmanHunterDied(this);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
