using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampiricCrown : ArtifactEffect
{
    float waitPeriod = 0;
    [SerializeField] DisplayItem displayItem;
    Coroutine mainLoopInstant;
    [SerializeField] GameObject bloodDrop;
    [SerializeField] AudioSource bloodAudio;

    public override void artifactEquipped()
    {
        mainLoopInstant = StartCoroutine(mainLoop());
    }

    public override void artifactUnequipped()
    {
        StopCoroutine(mainLoopInstant);
    }

    IEnumerator mainLoop()
    {
        waitPeriod = 8;

        while (true)
        {
            if (waitPeriod < 8)
            {
                waitPeriod += Time.deltaTime;
            }
            else
            {
                if (!PlayerProperties.playerScript.enemiesDefeated)
                {
                    if (!EnemyPool.isPoolEmpty())
                    {
                        float distance = int.MaxValue;
                        Enemy targetEnemy = null;
                        foreach (Enemy enemy in EnemyPool.enemyPool)
                        {
                            if(Vector2.Distance(enemy.transform.position, PlayerProperties.playerShipPosition) < distance)
                            {
                                distance = Vector2.Distance(enemy.transform.position, PlayerProperties.playerShipPosition);
                                targetEnemy = enemy;
                            }
                        }

                        targetEnemy.dealDamage(4);
                        PlayerProperties.playerScript.healPlayer(400);
                        float offset = Random.Range(0, 90);
                        for (int i = 0; i < 3; i++)
                        {
                            float angle = (offset + i * 120 + Random.Range(-20, 20)) * Mathf.Deg2Rad; 
                            Instantiate(bloodDrop, targetEnemy.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(0.5f, 1f), Quaternion.identity);
                        }
                        bloodAudio.Play();
                        waitPeriod = 0;
                    }
                }
            }

            yield return null;
        }
    }
}
