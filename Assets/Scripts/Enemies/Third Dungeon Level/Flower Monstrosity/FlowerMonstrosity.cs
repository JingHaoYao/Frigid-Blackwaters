using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerMonstrosity : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    public Sprite[] viewSprites;
    public GameObject[] hitBoxes;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private bool withinRange = false;
    private float travelAngle;
    private float pokePeriod = 1.5f;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    [SerializeField] private float withinRangeRadius = 1.9f;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    bool attacking = false;
    [SerializeField] private AudioSource waterSound;

    public GameObject damagingSplash;
    Camera mainCamera;

    int whatView = 1;
    int mirror = 1;

    private bool bloomed = false;

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

    IEnumerator poke()
    {
        animator.enabled = true;
        attacking = true;
        int whichViewCommit = whatView;
        animator.SetTrigger("Attack" + whatView);
        yield return new WaitForSeconds(7f / 12f);
        hitBoxes[whichViewCommit - 1].SetActive(true);
        attackAudio.Play();
        yield return new WaitForSeconds(1f / 12f);
        StartCoroutine(spawnSplashes());
        hitBoxes[whichViewCommit - 1].SetActive(false);
        yield return new WaitForSeconds(2f / 12f);
        animator.enabled = false;
    }

    IEnumerator spawnSplashes()
    {
        waterSound.Play();
        for (int i = 0; i < 6; i++)
        {
            float angleToConsider = i * 60 * Mathf.Deg2Rad;
            Vector3 spawnLocation = transform.position + (new Vector3(Mathf.Cos(angleToConsider), Mathf.Sin(angleToConsider)) * 1.5f);
            if (Mathf.Abs(spawnLocation.x - mainCamera.transform.position.x) > 8 || Mathf.Abs(spawnLocation.y - mainCamera.transform.position.y) > 8)
            {
                continue;
            }
            GameObject splashInstant = Instantiate(damagingSplash, spawnLocation, Quaternion.identity);
            splashInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }

        yield return new WaitForSeconds(0.3f);

        waterSound.Play();
        for (int i = 0; i < 8; i++)
        {
            float angleToConsider = i * 45 * Mathf.Deg2Rad;
            Vector3 spawnLocation = transform.position + new Vector3(Mathf.Cos(angleToConsider), Mathf.Sin(angleToConsider)) * 3;
            if(Mathf.Abs(spawnLocation.x - mainCamera.transform.position.x) > 8 || Mathf.Abs(spawnLocation.y - mainCamera.transform.position.y) > 8)
            {
                continue;
            }

            GameObject splashInstant = Instantiate(damagingSplash, spawnLocation, Quaternion.identity);
            splashInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }

        if (bloomed)
        {
            yield return new WaitForSeconds(0.3f);
            waterSound.Play();
            for (int i = 0; i < 8; i++)
            {
                float angleToConsider = i * 45 * Mathf.Deg2Rad;
                Vector3 spawnLocation = transform.position + new Vector3(Mathf.Cos(angleToConsider), Mathf.Sin(angleToConsider)) * 4.5f;
                if (Mathf.Abs(spawnLocation.x - mainCamera.transform.position.x) > 8 || Mathf.Abs(spawnLocation.y - mainCamera.transform.position.y) > 8)
                {
                    continue;
                }

                GameObject splashInstant = Instantiate(damagingSplash, spawnLocation, Quaternion.identity);
                splashInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }

        attacking = false;
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Bloom Status Effect" || newStatus.name == "Bloom Status Effect(Clone)")
        {
            bloomed = true;
        }
    }

    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 4;
            mirror = -1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 4;
            mirror = 1;
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

    float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    private void Start()
    {
        animator.enabled = false;
        mainCamera = Camera.main;
    }

    void Update()
    {
        pickRendererLayer();
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = PlayerProperties.playerShipPosition;
        Vector3 targetPos = Vector3.zero;
        if (path[0] != null)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (withinRange == false)
        {
            if (attacking == false)
            {
                moveTowards(travelAngle);
            }
            
            pokePeriod = 1.5f;

            pickSpritePeriod += Time.deltaTime;
            if (pickSpritePeriod >= 0.2f)
            {
                pickView(angleToShip());
                spriteRenderer.sprite = viewSprites[whatView - 1];
                transform.localScale = new Vector3(4 * mirror, 4);
                pickSpritePeriod = 0;
            }
        }
        else
        {
            pokePeriod += Time.deltaTime;
            rigidBody2D.velocity = Vector3.zero;
            if (attacking == false)
            {
                pickView(angleToShip());
                transform.localScale = new Vector3(4 * mirror, 4);
                spriteRenderer.sprite = viewSprites[whatView - 1];
            }

            if (pokePeriod >= 1.5f && stopAttacking == false)
            {
                pokePeriod = 0;
                StartCoroutine(poke());
            }
        }

        if (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) <= withinRangeRadius)
        {
            withinRange = true;
        }
        else
        {
            withinRange = false;
        }
        spawnFoam();
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
}
