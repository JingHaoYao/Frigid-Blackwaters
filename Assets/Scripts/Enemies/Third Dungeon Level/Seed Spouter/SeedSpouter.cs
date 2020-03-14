using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSpouter : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    public Sprite[] viewSprites;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private float travelAngle;
    public GameObject deadSpouter;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    private float offSet;

    public GameObject seedProjectile;

    bool attacking = false;

    int whatView = 1;
    int mirror = 1;

    private bool bloomed = false;
    private float attackPeriod = 0;

    private Vector3 randomPos;

    Camera mainCamera;

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

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Bloom Status Effect" || newStatus.name == "Bloom Status Effect(Clone)")
        {
            bloomed = true;
        }
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
    }

    private void Start()
    {
        animator.enabled = false;
        offSet = Random.Range(0, 2) * 45;
        mainCamera = Camera.main;
        randomPos = pickRandPos();
    }

    void Update()
    {
        travelLocation();
        spawnFoam();
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
                transform.localScale = new Vector3(3 * mirror, 3);
            }
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            if (attacking == false && Vector2.Distance(transform.position, randomPos) < 1f)
            {
                StartCoroutine(podAttack());
            }
        }
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    IEnumerator podAttack()
    {
        animator.enabled = true;
        attacking = true;
        pickView(angleToShip());
        transform.localScale = new Vector3(3 * mirror, 3);
        animator.SetTrigger("Attack" + whatView);
        yield return new WaitForSeconds(4 / 12f);
        attackAudio.Play();
        launchSeeds();
        yield return new WaitForSeconds(5 / 12f);
        randomPos = pickRandPos();
        animator.enabled = false;
        attacking = false;
    }

    private void launchSeeds()
    {
        if (bloomed == false)
        {
            for (int i = 0; i < 4; i++)
            {
                float radAngle = (i * 90 + offSet) * Mathf.Deg2Rad;
                GameObject seedInstant = Instantiate(seedProjectile, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
                Vector3 possibleTargetPosition = transform.position + new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle)) * 6;
                seedInstant.GetComponent<Thornball>().targetLocation =
                    new Vector3(
                        Mathf.Clamp(possibleTargetPosition.x, mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8),
                        Mathf.Clamp(possibleTargetPosition.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8)
                    );
                seedInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            float radAngle = (i * 45);
            GameObject seedInstant = Instantiate(seedProjectile, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
            Vector3 possibleTargetPosition = transform.position + new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle)) * 6;
            seedInstant.GetComponent<Thornball>().targetLocation =
                new Vector3(
                    Mathf.Clamp(possibleTargetPosition.x, mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8),
                    Mathf.Clamp(possibleTargetPosition.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8)
                );
            seedInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
    }

    Vector3 pickRandPos()
    {
        float randX;
        float randY;
        if (Random.Range(0, 2) == 1)
        {
            randX = transform.position.x + Random.Range(5.0f, 7.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 7.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-7.0f, -5.0f);
            }
        }
        else
        {
            randX = transform.position.x + Random.Range(-7.0f, -5.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 7.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-7.0f, -5.0f);
            }
        }

        Vector3 randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        while (Physics2D.OverlapCircle(randPos, .5f) || Vector2.Distance(randPos, transform.position) < 2)
        {
            if (Random.Range(0, 2) == 1)
            {
                randX = transform.position.x + Random.Range(5.0f, 7.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 7.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-7.0f, -5.0f);
                }
            }
            else
            {
                randX = transform.position.x + Random.Range(-7.0f, -5.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 7.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-7.0f, -5.0f);
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
        GameObject deadPirate = Instantiate(deadSpouter, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
