using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockShells : ArtifactEffect
{
    List<BranchLightning> branchLightnings = new List<BranchLightning>();
    [SerializeField] GameObject branchLightningPrefab;
    [SerializeField] GameObject boltPrefab;
    [SerializeField] AudioSource lightningAudio;

    List<Enemy> enemyList = new List<Enemy>();

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        summonLightning(enemy);
    }

    IEnumerator delayUntilClear(Enemy enemy)
    {
        yield return new WaitForSeconds(1f);
        enemyList.Remove(enemy);
    }

    void summonLightning(Enemy targetEnemy)
    {
        lightningAudio.Play();
        List<Enemy> enemyPoolInstant = new List<Enemy>();
        foreach(Enemy enemy in EnemyPool.enemyPool)
        {
            enemyPoolInstant.Add(enemy);
        }

        enemyList.Add(targetEnemy);
        StartCoroutine(delayUntilClear(targetEnemy));

        foreach(Enemy enemy in enemyPoolInstant)
        {
            if(enemy != targetEnemy && Vector2.Distance(targetEnemy.transform.position, enemy.transform.position) <= 5 && !enemyList.Contains(enemy))
            {
                enemyList.Add(enemy);
                GameObject branchObj = (GameObject)GameObject.Instantiate(branchLightningPrefab);
                BranchLightning branchLightning = branchObj.GetComponent<BranchLightning>();
                branchLightning.Initialize(targetEnemy.transform.position, enemy.transform.position, boltPrefab);
                branchLightnings.Add(branchLightning);
                enemy.dealDamage(5);
                StartCoroutine(delayUntilClear(enemy));
            }
        }
    }

    private void Update()
    {
        for (int i = branchLightnings.Count - 1; i >= 0; i--)
        {
            BranchLightning branchComponent = branchLightnings[i];

            if (branchComponent.IsComplete)
            {
                Destroy(branchLightnings[i]);
                branchLightnings.RemoveAt(i);

                continue;
            }

            branchComponent.UpdateBranch();
            branchComponent.Draw();
        }
    }
}
