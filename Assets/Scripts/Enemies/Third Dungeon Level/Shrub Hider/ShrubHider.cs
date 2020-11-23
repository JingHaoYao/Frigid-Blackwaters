using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrubHider : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    public Sprite[] viewSprites;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private float travelAngle;
    public GameObject deadTree;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private GameObject hidingContentContainer;
    bool attacking = false;

    public GameObject healParticles;

    float coolDownPeriod = 0;

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

    IEnumerator hideAndRisePlant()
    {
        rigidBody2D.velocity = Vector3.zero;
        animator.enabled = true;
        attacking = true;
        animator.SetTrigger("Hide");
        this.armorMitigation += bloomed ? 5 : 3;
        yield return new WaitForSeconds(10f / 12f);
        hidingContentContainer.SetActive(true);
        yield return new WaitForSeconds(2 / 12f);
    
        for(int i = 0; i < (bloomed ? 6 : 4); i++)
        {
            this.heal(1);
            GameObject particlesInstant = Instantiate(healParticles, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            particlesInstant.GetComponent<FollowObject>().objectToFollow = this.gameObject;
            yield return new WaitForSeconds(1f);
        }

        animator.SetTrigger("Rise");
        yield return new WaitForSeconds(2f / 12f);
        hidingContentContainer.SetActive(false);
        yield return new WaitForSeconds(10f / 12f);
        animator.enabled = false;
        attacking = false;
        coolDownPeriod = 2;
    }

    void Update()
    {
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

        float angleToShip = (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;

        if (attacking == false)
        {
            moveTowards(travelAngle);
            if (pickSpritePeriod > 0.2f)
            {
                pickView(travelAngle);
                spriteRenderer.sprite = viewSprites[whatView - 1];
                transform.localScale = new Vector3(4 * mirror, 4);
                pickSpritePeriod = 0;
            }

            if(coolDownPeriod > 0)
            {
                coolDownPeriod -= Time.deltaTime;
            }
        }

        if(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 2f)
        {
            if(attacking == false)
            {
                StartCoroutine(hideAndRisePlant());
            }
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
        GameObject deadPirate = Instantiate(deadTree, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();

        if(((float)this.health / this.maxHealth <= 0.6f) && coolDownPeriod <= 0 && attacking == false)
        {
            StartCoroutine(hideAndRisePlant());
        }
    }
}
