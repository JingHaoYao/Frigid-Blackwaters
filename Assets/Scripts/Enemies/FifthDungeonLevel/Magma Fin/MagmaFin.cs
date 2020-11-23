using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaFin : Enemy
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
    bool isAttacking = false;
    int whatView = 1;
    int mirror = 1;
    [SerializeField] LayerMask solidObstacleLayerMask;
    [SerializeField] Sprite[] viewSprites;
    [SerializeField] GameObject damageHitbox;
    [SerializeField] GameObject floorFire;
    Vector3 pastPosition;
    int prevView = 0;

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
    }

    void Update()
    {
        spawnFoam();
        travelLocation();
    }

    void spawnFoamDuringDash(float angle)
    {
        if(Vector2.Distance(transform.position, pastPosition) > 0.75f)
        {
            Instantiate(floorFire, transform.position, Quaternion.identity);
            pastPosition = transform.position;
        }

        foamTimer += Time.deltaTime;
        if (foamTimer >= 0.05f * speed / 3f)
        {
            foamTimer = 0;
            GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, angle + 90));
        }
    }

    IEnumerator diveAttack()
    {
        rigidBody2D.velocity = Vector3.zero;
        isAttacking = true;
        pickView(angleToShip());
        float angleAttack = angleToShip();
        animator.enabled = true;
        animator.SetTrigger("Submerge" + whatView);
        yield return new WaitForSeconds(5 / 12f);

        if (stopAttacking == false)
        {
            pastPosition = transform.position;
            damageHitbox.SetActive(true);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(angleAttack * Mathf.Deg2Rad), Mathf.Sin(angleAttack * Mathf.Deg2Rad)), 20, solidObstacleLayerMask);
            float time = Vector2.Distance(transform.position, hit.point) / 10f;
            LeanTween.move(this.gameObject, hit.point, Vector2.Distance(transform.position, hit.point) / 10f).setEaseInOutQuad().setOnUpdate(spawnFoamDuringDash);

            animator.SetTrigger("Idle");
            attackAudio.Play();

            yield return new WaitForSeconds(time);
        }

        damageHitbox.SetActive(false);

        animator.SetTrigger("Emerge");

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

        if (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 4 && isAttacking == false)
        {
            StartCoroutine(diveAttack());
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
        damageAudio.Play();
    }
}
