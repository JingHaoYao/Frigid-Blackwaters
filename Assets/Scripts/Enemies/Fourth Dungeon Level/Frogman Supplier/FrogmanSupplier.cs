using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FrogmanSupplier : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    [SerializeField] Sprite[] viewSprites;
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
    public GameObject bullet;
    [SerializeField] InvisibilityEnemyController invisController;
    float attackPeriod = 0;
    [SerializeField] GameObject frogmanHunter;
    List<FrogmanHunter> spawnedFrogmen = new List<FrogmanHunter>();
    Camera mainCamera;
    private FrogmanHunter targetFrogman;
    private bool allFrogmenDefeated = false;
    [SerializeField] GameObject fogRevealer;
    
    void spawnAllFrogmen()
    {
        float angleToCenterOfRoom = Mathf.Atan2(mainCamera.transform.position.y - transform.position.y, mainCamera.transform.position.x - transform.position.x);

        for (int i = 0; i < 3; i++) {
            GameObject frogmanHunterInstant = Instantiate(frogmanHunter, transform.position + new Vector3(Mathf.Cos(angleToCenterOfRoom - (-45 + 45 * i) * Mathf.Deg2Rad), Mathf.Sin(angleToCenterOfRoom - (-45 + 45 * i) * Mathf.Deg2Rad)) * 2, Quaternion.identity);
            FrogmanHunter frogmanHunterScript = frogmanHunterInstant.GetComponent<FrogmanHunter>();
            frogmanHunterScript.Initialize(this);
            spawnedFrogmen.Add(frogmanHunterScript);
            EnemyPool.addEnemy(frogmanHunterScript);
        }
    }

    public void frogmanHunterDied(FrogmanHunter hunter)
    {
        spawnedFrogmen.Remove(hunter);
        if(spawnedFrogmen.Count <= 0)
        {
            allFrogmenDefeated = true;
        }
        else
        {
            pickTargetFrogman();
        }
    }

    public void pickTargetFrogman()
    {
        targetFrogman = spawnedFrogmen[0];
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
        GameObject bulletInstant = Instantiate(bullet, transform.position + returnBulletSpawnPosition(), Quaternion.identity);
        bulletInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        bulletInstant.GetComponent<SkeletalMusketRound>().angleTravel = angleAttack * Mathf.Deg2Rad;
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
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 4;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 5;
        }
        else if (angleOrientation >= 180 && angleOrientation < 240)
        {
            whatView = 6;
        }
        else if (angleOrientation >= 240 && angleOrientation < 300)
        {
            whatView = 1;
        }
        else
        {
            whatView = 2;
        }
    }

    private void Start()
    {
        animator.enabled = false;
        mainCamera = Camera.main;
        spawnAllFrogmen();
        pickTargetFrogman();
    }

    void Update()
    {
        spawnFoam();
        travelLocation();
    }

    Vector3 returnBulletSpawnPosition()
    {
        switch (whatView) {

            case 1:
                return new Vector3(0.6533f, -1.275f);
                break;
            case 2:
                return new Vector3(1.928f, -0.233f);
            case 3:
                return new Vector3(2.095f, 0.223f);
            case 4:
                return new Vector3(0.513f, 1.507f);
            case 5:
                return new Vector3(-1.015f, 0.174f);
            case 6:
                return new Vector3(-0.651f, -0.146f);
            default:
                return Vector3.zero;
        }
    }

    IEnumerator launchProjectiles()
    {
        animator.enabled = true;
        isAttacking = true;
        pickView(angleToShip());
        fogRevealer.SetActive(true);
        float angleAttack = angleToShip();
        Vector3 targetPosition = PlayerProperties.playerShipPosition;
        animator.SetTrigger("Fire" + whatView);
        yield return new WaitForSeconds(3 / 12f);
        attackAudio.Play();
        if (stopAttacking == false)
            spawnProjectiles(angleAttack);
        yield return new WaitForSeconds(4 / 12f);
        isAttacking = false;
        animator.enabled = false;
        fogRevealer.SetActive(false);
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = allFrogmenDefeated ? PlayerProperties.playerShipPosition : targetFrogman.transform.position;

        Vector3 targetPos = Vector3.zero;
        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (isAttacking == false)
        {
            if (attackPeriod < 2)
            {
                attackPeriod += Time.deltaTime;
            }
            else
            {
                StartCoroutine(launchProjectiles());
                attackPeriod = 0;
            }
        }

        if (path != null
            && path.Count > 0
            && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f
            && (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 3 || !allFrogmenDefeated)
            && isAttacking == false)
        {
            moveTowards(travelAngle);

            pickSpritePeriod += Time.deltaTime;
            if (pickSpritePeriod >= 0.2f)
            {
                pickView(angleToShip());
                pickSpritePeriod = 0;
                pickSprite(whatView);
            }
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }
    }

    void pickSprite(int whatView)
    {
        spriteRenderer.sprite = viewSprites[whatView - 1];
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
        foreach(FrogmanHunter hunter in spawnedFrogmen)
        {
            hunter.SupplierDefeated();
        }
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
