using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivorousTangleFiendFlowerTurret : Enemy
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioSource takeDamageAudio, fireSeedAudio, deathAudio;
    [SerializeField] BoxCollider2D takeDamageHitBox;
    [SerializeField] Animator animator;
    private Camera mainCamera;
    [SerializeField] private GameObject seedProjectile;
    public CarnivorousTangleFiend fiend;

    void Start()
    {
        mainCamera = Camera.main;
        EnemyPool.addEnemy(this);
    }

    private void Update()
    {
        if (health > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, angleToShip);
        }
    }

    IEnumerator flowerTurretAttack(int numberShots)
    {
        if (stopAttacking == false && health > 0)
        {
            for (int i = 0; i < numberShots; i++)
            {
                if(stopAttacking == true)
                {
                    break;
                }

                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(4 / 12f);
                fireSeedAudio.Play();
                GameObject seed = Instantiate(seedProjectile, transform.position + new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad)) * 2, Quaternion.identity);
                seed.GetComponent<BasicProjectile>().angleTravel = angleToShip;
                seed.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                yield return new WaitForSeconds(6 / 12f);
            }
        }
        animator.SetTrigger("Idle");
    }

    public void spitSeed(int numberShots)
    {
        StartCoroutine(flowerTurretAttack(numberShots));
    }


    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        takeDamageHitBox.enabled = false;
        animator.SetTrigger("Death");
        deathAudio.Play();
        fiend.turrets.Remove(this);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        takeDamageAudio.Play();
        SpawnArtifactKillsAndGoOnCooldown();
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
            if (health > 0)
            {
                int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
                fiend.updateDamageToBoss(damageDealt, health);
                dealDamage(damageDealt);
            }
        }
    }
}
