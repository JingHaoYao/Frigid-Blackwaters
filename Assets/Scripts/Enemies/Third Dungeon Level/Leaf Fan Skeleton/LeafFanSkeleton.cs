using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafFanSkeleton : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    public Sprite[] viewSprites;
    public GameObject[] hitBoxes;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private float travelAngle;
    private float pokePeriod = 1.5f;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private AudioSource waveAudio;
    bool attacking = false;

    int whatView = 1;
    int mirror = 1;

    private float attackPeriod = 0;

    public GameObject wave1, wave2, wave3, wave4, wave5;

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

    IEnumerator fanWave()
    {
        animator.enabled = true;
        attacking = true;
        rigidBody2D.velocity = Vector3.zero;
        float angleAttack = angleToShip();
        attackAudio.Play();
        animator.SetTrigger("Attack" + whatView);
        yield return new WaitForSeconds(4f / 12f);
        hitBoxes[whatView - 1].SetActive(true);
        waveAudio.Play();
        yield return new WaitForSeconds(1f / 12f);
        for(int i = 0; i < 3; i++)
        {
            float angleToConsider = (360 + angleAttack - 5 + 5 * i) % 360;
            pickWaveAndSpawn(angleToConsider, transform.position + new Vector3(Mathf.Cos(angleToConsider * Mathf.Deg2Rad), Mathf.Sin(angleToConsider * Mathf.Deg2Rad)) * 0.75f);
        }
        hitBoxes[whatView - 1].SetActive(false);
        yield return new WaitForSeconds(8f / 12f);
        animator.enabled = false;
        attacking = false;
    }

    void pickWaveAndSpawn(float angleOrientation, Vector3 spawnPosition)
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            GameObject wave = Instantiate(wave4, spawnPosition, Quaternion.identity);
            wave.transform.localScale = new Vector3(-2, 2);
            wave.GetComponent<LeafFanSkeletonWave>().angleTravel = angleOrientation;
            wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            GameObject wave = Instantiate(wave5, spawnPosition, Quaternion.identity);
            wave.GetComponent<LeafFanSkeletonWave>().angleTravel = angleOrientation;
            wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            GameObject wave = Instantiate(wave4, spawnPosition, Quaternion.identity);
            wave.GetComponent<LeafFanSkeletonWave>().angleTravel = angleOrientation;
            wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            GameObject wave = Instantiate(wave3, spawnPosition, Quaternion.identity);
            wave.GetComponent<LeafFanSkeletonWave>().angleTravel = angleOrientation;
            wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            GameObject wave = Instantiate(wave2, spawnPosition, Quaternion.identity);
            wave.GetComponent<LeafFanSkeletonWave>().angleTravel = angleOrientation;
            wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            GameObject wave = Instantiate(wave1, spawnPosition, Quaternion.identity);
            wave.GetComponent<LeafFanSkeletonWave>().angleTravel = angleOrientation;
            wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            GameObject wave = Instantiate(wave2, spawnPosition, Quaternion.identity);
            wave.transform.localScale = new Vector3(-2, 2);
            wave.GetComponent<LeafFanSkeletonWave>().angleTravel = angleOrientation;
            wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else
        {
            GameObject wave = Instantiate(wave3, spawnPosition, Quaternion.identity);
            wave.transform.localScale = new Vector3(-2, 2);
            wave.GetComponent<LeafFanSkeletonWave>().angleTravel = angleOrientation;
            wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
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

    float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void Update()
    {
        pickRendererLayer();
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

        if (attacking == false)
        {
            moveTowards(travelAngle);
            animator.enabled = false;
            attackPeriod += Time.deltaTime;
            if (pickSpritePeriod >= 0.2f)
            {
                spriteRenderer.sprite = viewSprites[whatView - 1];
                transform.localScale = new Vector3(4 * mirror, 4);
                pickView(angleToShip());
                pickSpritePeriod = 0;
            }

            if(attackPeriod > 3 && stopAttacking == false)
            {
                StartCoroutine(fanWave());
                attackPeriod = 0;
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
        GameObject deadPirate = Instantiate(deadSpearman, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
