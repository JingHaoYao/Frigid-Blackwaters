using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivorousTangleFiendSpikeFlower : Enemy
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioSource takeDamageAudio, deathAudio;
    [SerializeField] BoxCollider2D takeDamageHitBox;
    [SerializeField] GameObject vineSpike1, vineSpike2;
    [SerializeField] Animator animator;
    private Camera mainCamera;
    public CarnivorousTangleFiend fiend;


    IEnumerator spawnVineSpikes()
    {
        if (stopAttacking == false && health > 0)
        {
            int numberSpikesSpawned = 0;
            float angleToShip = Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x);

            for (int i = 0; i < 7; i++)
            {
                if(stopAttacking == true)
                {
                    break;
                }

                if (i < 5)
                {
                    angleToShip = Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x);
                }

                Vector3 positionToSpawn = transform.position + new Vector3(Mathf.Cos(angleToShip), Mathf.Sin(angleToShip)) * (i + 1) * 2;
                if (checkIfPositionIsValid(positionToSpawn))
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        GameObject spike = Instantiate(vineSpike1, positionToSpawn, Quaternion.identity);
                        spike.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                        if (Random.Range(0, 2) == 1)
                        {
                            Vector3 scale = spike.transform.localScale;
                            spike.transform.localScale = new Vector3(scale.x * -1, scale.y);
                        }
                    }
                    else
                    {
                        GameObject spike = Instantiate(vineSpike2, positionToSpawn, Quaternion.identity);
                        spike.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                        if (Random.Range(0, 2) == 1)
                        {
                            Vector3 scale = spike.transform.localScale;
                            spike.transform.localScale = new Vector3(scale.x * -1, scale.y);
                        }
                    }
                }
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    public void openFlower()
    {
        animator.SetTrigger("Open");
        StartCoroutine(spawnVineSpikes());
        takeDamageHitBox.enabled = true;
    }

    public void closeFlower()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Spike Flower Closed Idle"))
        {
            animator.SetTrigger("Close");
            takeDamageHitBox.enabled = false;
        }
    }

    bool checkIfPositionIsValid(Vector3 pos)
    {
        return Mathf.Abs(pos.x - mainCamera.transform.position.x) < 8.5f && Mathf.Abs(pos.y - mainCamera.transform.position.y) < 8.5f;
    }

    void Start()
    {
        mainCamera = Camera.main;
        takeDamageHitBox.enabled = false;
        EnemyPool.addEnemy(this);
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        takeDamageHitBox.enabled = false;
        animator.SetTrigger("Death");
        deathAudio.Play();
        fiend.spikeFlowers.Remove(this);
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
