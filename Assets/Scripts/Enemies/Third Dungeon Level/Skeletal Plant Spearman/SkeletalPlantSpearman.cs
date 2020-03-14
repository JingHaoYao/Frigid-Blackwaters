using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalPlantSpearman : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    public Sprite[] viewSprites;
    public GameObject[] hitBoxes;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private bool withinRange = false, touchingShip = false;
    private float travelAngle;
    private float lastTravelAngle;
    private float speedBonus;
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

    int whatView = 1;
    int mirror = 1;

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
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * (speed + speedBonus);
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
        attackAudio.Play();
        animator.SetTrigger("Attack" + whatView);
        yield return new WaitForSeconds(4f / 12f);
        hitBoxes[whatView - 1].SetActive(true);
        yield return new WaitForSeconds(1f / 12f);
        hitBoxes[whatView - 1].SetActive(false);
        yield return new WaitForSeconds(1f / 12f);
        animator.enabled = false;
        attacking = false;
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
        Vector3 targetPos = Vector3.zero;
        if (path[0] != null)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
        if(travelAngle != lastTravelAngle)
        {
            lastTravelAngle = travelAngle;
            speedBonus = 0;
        }
        else
        {
            speedBonus = Mathf.Clamp(speedBonus + Time.deltaTime * 2, 0, 6);
        }

        pickSpritePeriod += Time.deltaTime;

        float angleToShip = (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;

        if (withinRange == false && touchingShip == false && attacking == false)
        {
            moveTowards(travelAngle);
            animator.enabled = false;
            pokePeriod = 1.5f;
            if (pickSpritePeriod >= 0.2f)
            {
                spriteRenderer.sprite = viewSprites[whatView - 1];
                transform.localScale = new Vector3(4 * mirror, 4);
                pickView(angleToShip);
                pickSpritePeriod = 0;
            }
        }
        else
        {
            pokePeriod += Time.deltaTime;
            rigidBody2D.velocity = Vector3.zero;
            if (attacking == false)
            {
                pickView(angleToShip);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerProperties.playerShip)
        {
            touchingShip = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerProperties.playerShip)
        {
            touchingShip = false;
        }
    }
}
