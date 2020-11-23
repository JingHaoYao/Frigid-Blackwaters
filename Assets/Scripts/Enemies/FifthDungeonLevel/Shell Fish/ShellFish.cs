using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellFish : Enemy
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
    [SerializeField] Sprite[] viewSprites;
    [SerializeField] GameObject smallSpike, largeSpike;
    List<ShellFishSpike> spikeInstances = new List<ShellFishSpike>();
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

        if (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 3 && isAttacking == false && stopAttacking == false)
        {
            StartCoroutine(spikeAttack());
        }

        if (isAttacking == false)
        {
            pickIdleAnimation();
        }
    }

    IEnumerator spikeAttack()
    {
        animator.enabled = true;
        isAttacking = true;
        rigidBody2D.velocity = Vector3.zero;
        pickView(angleToShip());
        animator.SetTrigger("Submerge" + whatView);
        attackAudio.Play();
        this.moveable = false;
        
        yield return new WaitForSeconds(0.5f);
        
        for(int i = 0; i < 6; i++)
        {
            Vector3 position = transform.position + new Vector3(Mathf.Cos(i * 60 * Mathf.Deg2Rad), Mathf.Sin(i * 60 * Mathf.Deg2Rad));
            GameObject spikeInstant = Instantiate(largeSpike, position, Quaternion.identity);
            spikeInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            spikeInstances.Add(spikeInstant.GetComponent<ShellFishSpike>());
        }

        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < 8; i++)
        {
            Vector3 position = transform.position + new Vector3(Mathf.Cos(i * 45 * Mathf.Deg2Rad), Mathf.Sin(i * 45 * Mathf.Deg2Rad)) * 1.5f;
            GameObject spikeInstant = Instantiate(largeSpike, position, Quaternion.identity);
            spikeInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            spikeInstances.Add(spikeInstant.GetComponent<ShellFishSpike>());
        }

        yield return new WaitForSeconds(1f);

        while(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 4)
        {
            yield return null;
        }

        foreach(ShellFishSpike spike in spikeInstances)
        {
            spike.Sink();
        }
        spikeInstances.Clear();

        animator.SetTrigger("Emerge");

        yield return new WaitForSeconds(0.5f);

        this.moveable = true;
        animator.enabled = false;
        isAttacking = false;
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
