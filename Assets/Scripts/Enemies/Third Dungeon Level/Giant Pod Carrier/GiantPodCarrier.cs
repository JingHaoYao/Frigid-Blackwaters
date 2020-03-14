using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantPodCarrier : Enemy
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource damageAudio;
    [SerializeField] AudioSource whipAttackAudio;
    [SerializeField] AudioSource podLaunchAudio;
    [SerializeField] private GameObject whipHitBox;
    private List<AStarNode> path;

    private float foamTimer = 0;
    public GameObject waterFoam;

    public GameObject deadSkele;

    private bool isAttacking = false;

    [SerializeField] private AStarPathfinding aStarPathfinding;
    public GameObject podProjectile;

    [SerializeField] LayerMask detectingLayermask;
    Camera mainCamera;

    private bool bloomed;

    private float launchPodsPeriod = 0;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * speed / 3f)
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    IEnumerator launchPods(float waitDuration)
    {
        isAttacking = true;
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(5 / 12f);
        yield return new WaitForSeconds(waitDuration);
        animator.SetTrigger("Open");
        podLaunchAudio.Play();
        yield return new WaitForSeconds(4 / 12f);
        launchPods();
        yield return new WaitForSeconds(2 / 12f);
        animator.SetTrigger("Idle");
        isAttacking = false;
        launchPodsPeriod = 0;
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Bloom Status Effect" || newStatus.name == "Bloom Status Effect(Clone)")
        {
            bloomed = true;
        }
    }

    void launchPods()
    {
        for (int i = 0; i < (bloomed ? 5 : 3) ; i++)
        {
            GameObject podInstant = Instantiate(podProjectile, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            podInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            podInstant.GetComponent<GiantPodCarrierPod>().targetLocation = pickPodTargetPosition();
        }
    }

    Vector3 pickPodTargetPosition()
    {
        Vector3 randPos = new Vector3(mainCamera.transform.position.x + Random.Range(-7.5f, 7.5f), mainCamera.transform.position.y + Random.Range(-7.5f, 7.5f));
        while(Physics2D.OverlapCircle(randPos, 0.4f, detectingLayermask))
        {
            randPos = new Vector3(mainCamera.transform.position.x + Random.Range(-7.5f, 7.5f), mainCamera.transform.position.y + Random.Range(-7.5f, 7.5f));
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

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = PlayerProperties.playerShipPosition;
        Vector3 targetPos = Vector3.zero;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (path != null && path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 1 && isAttacking == false)
        {
            moveTowards(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }
    }

    private void Start()
    {
        whipHitBox.SetActive(false);
        mainCamera = Camera.main;
    }

    void Update()
    {
        spawnFoam();
        travelLocation();
        if (isAttacking == false && Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 2f)
        {
            isAttacking = true;
            StartCoroutine(whipAttack());
        }

        launchPodsPeriod += Time.deltaTime;

        if(launchPodsPeriod >= 8 && isAttacking == false)
        {
            StartCoroutine(launchPods(1.5f));
        }
    }

    IEnumerator whipAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Whip");
        yield return new WaitForSeconds(7f / 12f);
        whipAttackAudio.Play();
        whipHitBox.SetActive(true);
        yield return new WaitForSeconds(1 / 12f);
        whipHitBox.SetActive(false);
        yield return new WaitForSeconds(3 / 12f);
        whipHitBox.SetActive(true);
        yield return new WaitForSeconds(1 / 12f);
        whipHitBox.SetActive(false);
        yield return new WaitForSeconds(6/ 12f);
        isAttacking = false;
        animator.SetTrigger("Idle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public override void deathProcedure()
    {
        GameObject dead = Instantiate(deadSkele, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        damageAudio.Play();
        StartCoroutine(hitFrame());
    }
}
