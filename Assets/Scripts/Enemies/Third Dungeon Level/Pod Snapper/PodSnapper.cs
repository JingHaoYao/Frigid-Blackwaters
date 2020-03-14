using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodSnapper : Enemy
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource damageAudio;
    [SerializeField] AudioSource attackAudio;
    private List<AStarNode> path;

    public GameObject deadSkele;

    private float angleToShip;

    private bool bloomed = false;
    private bool isAttacking = false;

    [SerializeField] private AStarPathfinding aStarPathfinding;
    [SerializeField] private CircleCollider2D damageCollider;
    [SerializeField] private DamageHitBox damageHitBox;

    private float speedBonus = 0;
    private float lastTravelAngle;

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
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * (speed + speedBonus);
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

        if(travelAngle == lastTravelAngle)
        {
            if (speedBonus < 5)
            {
                speedBonus += Time.deltaTime;
            }
        }
        else
        {
            speedBonus = 0;
            lastTravelAngle = travelAngle;
        }

        if (path != null && path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f)
        {
            moveTowards(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 1)
        {
            travelLocation();
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            if (isAttacking == false && stopAttacking == false)
            {
                isAttacking = true;
                StartCoroutine(bite());
            }
        }
    }
   
    IEnumerator bite()
    {
        animator.SetTrigger("Snap");
        yield return new WaitForSeconds(3 / 12f);
        damageCollider.enabled = true;
        attackAudio.Play();
        yield return new WaitForSeconds(2 / 12f);
        damageCollider.enabled = false;
        yield return new WaitForSeconds(2 / 12f);
        animator.SetTrigger("Idle");
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Bloom Status Effect" || newStatus.name == "Bloom Status Effect(Clone)")
        {
            damageHitBox.damageAmount = Mathf.RoundToInt(damageHitBox.damageAmount * 1.5f);
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
