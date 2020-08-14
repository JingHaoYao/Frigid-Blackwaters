using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinBlade : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject bloodTrail;
    [SerializeField] GameObject speedTrail;
    [SerializeField] Collider2D damageCollider;

    [SerializeField] AudioSource finBladeCutSound;

    List<GameObject> spawnedEffects = new List<GameObject>();

    float currentSpeedBonus = 0;

    private void Start()
    {
        ResetAnimationsAndSpawnedEffects();
    }

    public void ResetAnimationsAndSpawnedEffects()
    {
        if (PlayerUpgrades.finBladeUpgrades.Count >= 4)
        {
            if (PlayerUpgrades.finBladeUpgrades[3] == "soul_reaver_upgrade")
            {
                animator.SetTrigger("VampiricIdle");
            }
            else
            {
                animator.SetTrigger("SwiftIdle");
            }
        }
        else
        {
            animator.SetTrigger("RegularIdle");
        }

        foreach(GameObject instant in spawnedEffects)
        {
            Destroy(instant);
        }

        spawnedEffects.Clear();
    }

    private void Update()
    {
        damageCollider.enabled = PlayerProperties.playerScript.isShipMoving();
    }

    void summonSpeedTrail()
    {
        foreach(GameObject instant in spawnedEffects)
        {
            if (!instant.activeSelf)
            {
                instant.SetActive(true);  
                instant.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.35f);
                instant.transform.position = transform.position;
                instant.transform.rotation = transform.rotation;
                LeanTween.alpha(instant, 0, 1f);
                return;
            }
        }

        GameObject newInstant = Instantiate(speedTrail, transform.position, Quaternion.identity);
        newInstant.transform.rotation = transform.rotation;
        LeanTween.alpha(newInstant, 0, 1f);
        spawnedEffects.Add(newInstant);
    }

    void summonBloodTrail(Vector3 posToSpawn)
    {
        foreach (GameObject instant in spawnedEffects)
        {
            if (!instant.activeSelf)
            {
                instant.SetActive(true);
                instant.transform.position = posToSpawn;
            }
        }

        GameObject newInstant = Instantiate(bloodTrail, posToSpawn, Quaternion.identity);
        spawnedEffects.Add(newInstant);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyScript = collision.GetComponent<Enemy>();

        if(enemyScript != null)
        {
            dealDamage(enemyScript);
        }
    }

    IEnumerator speedIncrease(float speed)
    {
        PlayerProperties.playerScript.speedBonus += speed;
        currentSpeedBonus += speed;

        yield return new WaitForSeconds(2f);

        PlayerProperties.playerScript.speedBonus -= speed;
        currentSpeedBonus -= speed;
    }

    void ProcSpeedBonus(float speedBonus)
    {
        if(currentSpeedBonus + speedBonus > 6)
        {
            if (6 - currentSpeedBonus > 0)
            {
                StartCoroutine(speedIncrease(6 - currentSpeedBonus));
            }
        }
        else
        {
            StartCoroutine(speedIncrease(speedBonus));
        }
        StartCoroutine(speedTrailEffect());
    }

    IEnumerator speedTrailEffect()
    {
        for(int i = 0; i < 4; i++)
        {
            summonSpeedTrail();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void dealDamage(Enemy enemy)
    {
        finBladeCutSound.Play();
        switch (PlayerUpgrades.finBladeUpgrades.Count)
        {
            case 0:
                enemy.dealDamage(2 + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                break;
            case 1:
                enemy.dealDamage(2 + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                break;
            case 2:
                enemy.dealDamage(3 + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                break;
            case 3:
                enemy.dealDamage(4 + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                break;
            case 4:
                if(PlayerUpgrades.finBladeUpgrades[3] == "soul_reaver_upgrade")
                {
                    enemy.dealDamage(4 + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                    if(enemy.health <= 0)
                    {
                        PlayerProperties.playerScript.healPlayer(Mathf.RoundToInt((PlayerProperties.playerScript.shipHealthMAX - PlayerProperties.playerScript.shipHealth) * 0.1f));
                        summonBloodTrail(enemy.transform.position);
                    }
                }
                else
                {
                    float speed = PlayerProperties.playerScript.totalShipSpeed;
                    enemy.dealDamage(4 + Mathf.RoundToInt(speed / 2) + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                    ProcSpeedBonus(1f);
                }
                break;
            case 5:
                if (PlayerUpgrades.finBladeUpgrades[3] == "soul_reaver_upgrade")
                {
                    enemy.dealDamage(7 + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                    if (enemy.health <= 0)
                    {
                        PlayerProperties.playerScript.healPlayer(Mathf.RoundToInt((PlayerProperties.playerScript.shipHealthMAX - PlayerProperties.playerScript.shipHealth) * 0.2f));
                        summonBloodTrail(enemy.transform.position);
                    }
                }
                else
                {
                    float speed = PlayerProperties.playerScript.totalShipSpeed;
                    enemy.dealDamage(4 + Mathf.RoundToInt(speed / 2) + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                    ProcSpeedBonus(2f);
                }
                break;
            case 6:
                if (PlayerUpgrades.finBladeUpgrades[3] == "soul_reaver_upgrade")
                {
                    if((float)enemy.health / enemy.maxHealth <= 0.25f)
                    {
                        enemy.dealDamage(18 + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                    }
                    else
                    {
                        enemy.dealDamage(9 + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                    }

                    if (enemy.health <= 0)
                    {
                        PlayerProperties.playerScript.healPlayer(Mathf.RoundToInt((PlayerProperties.playerScript.shipHealthMAX - PlayerProperties.playerScript.shipHealth) * 0.2f));
                        summonBloodTrail(enemy.transform.position);
                    }
                }
                else
                {
                    float speed = PlayerProperties.playerScript.totalShipSpeed;
                    if (speed > 8)
                    {
                        enemy.dealDamage((4 + Mathf.RoundToInt(speed / 2)) * 2  + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                    }
                    else
                    {
                        enemy.dealDamage(4 + Mathf.RoundToInt(speed / 2) + PlayerProperties.playerScript.attackBonus + PlayerProperties.playerScript.conAttackBonus);
                    }
                    ProcSpeedBonus(2f);
                }
                break;
        }
    }

    private void OnDestroy()
    {
        PlayerProperties.playerScript.speedBonus -= currentSpeedBonus;
        foreach(GameObject effect in spawnedEffects)
        {
            Destroy(effect);
        }
    }
}
