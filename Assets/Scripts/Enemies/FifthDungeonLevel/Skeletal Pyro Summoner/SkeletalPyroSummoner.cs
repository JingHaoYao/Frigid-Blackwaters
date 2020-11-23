using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalPyroSummoner : Enemy
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
    private float offset = 0;
    public GameObject pyrotheumProjectile;
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
        randomPos = pickRandPos();
        animator.enabled = false;
        mainCamera = Camera.main;
        offset = Random.Range(0, 2) * 36;
    }

    void Update()
    {
        spawnFoam();
        travelLocation();
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
                StartCoroutine(callProjectiles());
            }
        }
    }

    Vector3 pickRandPos()
    {
        float randX;
        float randY;
        if (Random.Range(0, 2) == 1)
        {
            randX = transform.position.x + Random.Range(4.0f, 5.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(4.0f, 5.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-5.0f, -4.0f);
            }
        }
        else
        {
            randX = transform.position.x + Random.Range(-5.0f, -4.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(4.0f, 5.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-5.0f, -4.0f);
            }
        }

        Vector3 randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        while (Physics2D.OverlapCircle(randPos, .5f) || Vector2.Distance(randPos, transform.position) < 2)
        {
            if (Random.Range(0, 2) == 1)
            {
                randX = transform.position.x + Random.Range(4.0f, 5.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(4.0f, 5.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-5.0f, -4.0f);
                }
            }
            else
            {
                randX = transform.position.x + Random.Range(-5.0f, -4.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(4.0f, 5.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-5.0f, -4.0f);
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
        GameObject deadPirate = Instantiate(deadSpearman, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }

    IEnumerator callProjectiles()
    {
        animator.enabled = true;
        isAttacking = true;
        pickView(angleToShip());
        transform.localScale = new Vector3(4 * mirror, 4);

        animator.SetTrigger("Raise" + whatView);

        yield return new WaitForSeconds(10 / 12f);

        animator.SetTrigger("Idle");
        attackAudio.Play();

        if (stopAttacking == false)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x - 8, transform.position.y + 1.25f), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x + 8, transform.position.y + 1.25f), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(transform.position.x, mainCamera.transform.position.y + 8), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(transform.position.x, mainCamera.transform.position.y - 8), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                yield return new WaitForSeconds(1.5f);
            }
        }

        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Lower");
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        animator.enabled = false;
        randomPos = pickRandPos();
    }
}
