using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulSoul : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    public Sprite[] viewSprites;
    public GameObject deadSpearman;
    float pickSpritePeriod = 0;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    bool isAttacking = false;
    int whatView = 1;
    int mirror = 1;
    [SerializeField] GameObject damageHitbox;
    [SerializeField] LightAuraController eyeAuraController;
    private Vector3 targetPosition;
    private float angleOffset;
    private float distanceSpeedBonus;

    private float attackPeriod = 0;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction, float speedToTravel)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speedToTravel;
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
            eyeAuraController.SetRenderer(spriteRenderer.sortingOrder - 5);
        }
        else
        {
            eyeAuraController.SetRenderer(spriteRenderer.sortingOrder + 5);
        }
    }

    private void Start()
    {
        animator.enabled = false;
        angleOffset = Random.Range(0, 360);
    }

    void Update()
    {
        travelLocation();
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    IEnumerator dive()
    {
        animator.enabled = true;
        isAttacking = true;
        float angleAttack = angleToShip();
        pickView(angleAttack);
        animator.SetTrigger("Dive" + whatView);
        rigidBody2D.velocity = Vector3.zero;
        yield return new WaitForSeconds(4 / 12f);

        damageHitbox.SetActive(true);
        attackAudio.Play();
    
        LeanTween.value(0, 10, 0.3f).setOnUpdate((float val) => moveTowards(angleAttack, this.speed + val));

        yield return new WaitForSeconds(6 / 12f);

        LeanTween.value(8, 10, 0.333f).setOnUpdate((float val) => moveTowards(angleAttack, this.speed + val));

        yield return new WaitForSeconds(2 / 12f);

        angleOffset = Mathf.Atan2(transform.position.y - PlayerProperties.playerShipPosition.y, transform.position.x - PlayerProperties.playerShipPosition.x) * Mathf.Rad2Deg;
        damageHitbox.SetActive(false);
        animator.enabled = false;
        isAttacking = false;
    }

    void pickSpriteProcedure(float angleToLook)
    {
        pickSpritePeriod += Time.deltaTime;
        if (pickSpritePeriod >= 0.2f)
        {
            pickView(angleToLook);
            pickSpritePeriod = 0;
            spriteRenderer.sprite = viewSprites[whatView - 1];
            transform.localScale = new Vector3(3 * mirror, 3);
        }
    }

    void travelLocation()
    {
        if (isAttacking == false)
        {
            angleOffset += Time.deltaTime * 40;
            if (angleOffset > 360)
            {
                angleOffset = 0;
            }
            targetPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleOffset * Mathf.Deg2Rad), Mathf.Sin(angleOffset * Mathf.Deg2Rad)) * 4;

            distanceSpeedBonus = Vector2.Distance(transform.position, targetPosition);
            moveTowards(Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg, speed + distanceSpeedBonus);

            pickSpriteProcedure(angleToShip());

            attackPeriod += Time.deltaTime;
            if(attackPeriod > 3)
            {
                StartCoroutine(dive());
                attackPeriod = 0;
            }
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

