using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritedBlade : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    [SerializeField] private AStarPathfinding aStarPathfinding;
    private float travelAngle;
    public GameObject deadSpearman;
    List<AStarNode> path;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private GameObject damagingBox;
    [SerializeField] private Sprite[] viewSprites;
    bool isAttacking = false;
    int whatView = 1;
    int mirror = 1;
    private float attackPeriod = 0;
    [SerializeField] LightAuraController eyeAuraController;
    [SerializeField] Collider2D takeDamageCollider;
    [SerializeField] GameObject obstacleHitbox;
    private bool ghosted = true;

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Fogged Effect" || newStatus.name == "Fogged Effect(Clone)")
        {
            setDetectableMode();
        }
    }

    public override void statusRemoved(EnemyStatusEffect removedStatus)
    {
        if (removedStatus.name == "Fogged Effect" || removedStatus.name == "Fogged Effect(Clone)")
        {
            setUnDetectableMode();
        }
    }

    IEnumerator fadeOutRenderer(float val)
    {
        float currAlpha = spriteRenderer.color.a;
        while(spriteRenderer.color.a > val)
        {
            currAlpha -= Time.deltaTime * 2;
            spriteRenderer.color = new Color(1, 1, 1, currAlpha);
            yield return null;
        }
    }

    IEnumerator fadeInRenderer()
    {
        float currAlpha = spriteRenderer.color.a;
        while (spriteRenderer.color.a < 1)
        {
            currAlpha += Time.deltaTime * 2;
            spriteRenderer.color = new Color(1, 1, 1, currAlpha);
            yield return null;
        }
    }

    void setUnDetectableMode()
    {
        obstacleHitbox.SetActive(false);
        StartCoroutine(fadeOutRenderer(0.2f));
        takeDamageCollider.enabled = false;
        eyeAuraController.fadeOutLights(0.5f);
        ghosted = true;
    }

    void setDetectableMode()
    {
        obstacleHitbox.SetActive(true);
        StartCoroutine(fadeInRenderer());
        takeDamageCollider.enabled = true;
        eyeAuraController.fadeInLights(0.5f);
        ghosted = false;
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

        if (whatView > 2)
        {
            eyeAuraController.SetRenderer(spriteRenderer.sortingOrder - 3);
        }
        else
        {
            eyeAuraController.SetRenderer(spriteRenderer.sortingOrder + 3);
        }
        transform.localScale = new Vector3(mirror * 4f, 4f);
    }

    private void Start()
    {
        animator.enabled = false;
        setUnDetectableMode();
    }

    void Update()
    {
        travelLocation();
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void travelLocation()
    {
        if (ghosted == false)
        {
            path = aStarPathfinding.seekPath;
            aStarPathfinding.target = PlayerProperties.playerShipPosition;
            Vector3 targetPos = transform.position;

            if (path.Count > 0)
            {
                AStarNode pathNode = path[0];
                targetPos = pathNode.nodePosition;
            }

            float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

            if (path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f && Vector2.Distance(PlayerProperties.playerShipPosition, transform.position) > 2.2f)
            {
                moveTowards(travelAngle);
                pickViewSprite();
            }
            else
            {
                if (isAttacking == false)
                {
                    StartCoroutine(slice());
                }
            }
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            pickViewSprite();
        }
    }

    IEnumerator slice()
    {
        animator.enabled = true;
        isAttacking = true;
        float angleAttack = angleToShip();
        pickView(angleAttack);
        animator.SetTrigger("Attack" + whatView);
        yield return new WaitForSeconds(6 / 12f);
        damagingBox.transform.rotation = Quaternion.Euler(0, 0, angleAttack + (transform.localScale.x < 0 ? 180 : 0));
        damagingBox.SetActive(true);
        attackAudio.Play();
        yield return new WaitForSeconds(2 / 12f);
        damagingBox.SetActive(false);
        yield return new WaitForSeconds(5 / 12f);
        isAttacking = false;
        animator.enabled = false;
    }

    void pickViewSprite()
    {
        pickView(angleToShip());
        spriteRenderer.sprite = viewSprites[whatView - 1];
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
