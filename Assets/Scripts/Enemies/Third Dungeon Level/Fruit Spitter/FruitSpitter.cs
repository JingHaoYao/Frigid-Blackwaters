using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpitter : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private AudioSource rustleAudio;
    public GameObject death1, death2;
    public GameObject fruitProjectile;
    private bool isAttacking = false;
    private int whatView;
    private int mirror = 1;
    private bool bloomed = false;
    bool isHiding = false;

    private void Start()
    {
        StartCoroutine(hideAndRise(0.5f));
    }

    float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
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
    }

    public override void statusUpdated(EnemyStatusEffect newStatus)
    {
        if (newStatus.name == "Bloom Status Effect" || newStatus.name == "Bloom Status Effect (Clone)")
        {
            bloomed = true;
        }
    }

    IEnumerator spitFruits()
    {
        float attackAngle = angleToShip();
        pickView(attackAngle);
        animator.SetInteger("WhatView", whatView);
        transform.localScale = new Vector3(3 * mirror, 3);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(7 / 12f);
        attackAudio.Play();
        fruitAttack(attackAngle);
        yield return new WaitForSeconds(8 / 12f);
        //start next coroutine
        StartCoroutine(hideAndRise(bloomed ? 1 : 2));
    }

    IEnumerator hideAndRise(float waitDuration)
    {
        isHiding = true;
        animator.SetTrigger("Hiding");
        yield return new WaitForSeconds(4 / 12f + waitDuration);
        animator.SetTrigger("Rising");
        rustleAudio.Play();
        yield return new WaitForSeconds(7 / 12f);
        isHiding = false;
        StartCoroutine(spitFruits());
    }

    void fruitAttack(float attackAngle)
    {
        for(int i = 0; i < 5; i++)
        {
            float indexAngle = attackAngle - 15 + (7.5f * i);
            float convertedAngle = indexAngle * Mathf.Deg2Rad;
            GameObject fruitInstant = Instantiate(fruitProjectile, transform.position + new Vector3(Mathf.Cos(convertedAngle), Mathf.Sin(convertedAngle) + 0.75f) * 0.5f, Quaternion.identity);
            fruitInstant.GetComponent<FruitSpitterFruitProjectile>().angleTravel = indexAngle;
            fruitInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
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
        Instantiate(isHiding ? death1 : death2, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        damageAudio.Play();
        StartCoroutine(hitFrame());
    }
}
