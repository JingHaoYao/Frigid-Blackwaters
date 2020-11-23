using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoddessSpear : ArtifactEffect
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] GameObject goddessSpear;
    Coroutine coolDownRoutine;

    public override void artifactEquipped()
    {
        coolDownRoutine = StartCoroutine(coolDown());
    }

    public override void artifactUnequipped()
    {
        StopCoroutine(coolDownRoutine);
    }

    IEnumerator coolDown()
    {
        while (true)
        {
            PlayerProperties.durationUI.addTile(displayItem.displayIcon, 10f);
            yield return new WaitForSeconds(10f);

            while(EnemyPool.enemyPool.Count == 0)
            {
                yield return null;
            }

            float maxDistance = 0;
            Enemy targetEnemy = null;
            foreach(Enemy enemy in EnemyPool.enemyPool)
            {
                if(Vector2.Distance(enemy.transform.position, PlayerProperties.playerShipPosition) > maxDistance)
                {
                    maxDistance = Vector2.Distance(enemy.transform.position, PlayerProperties.playerShipPosition);
                    targetEnemy = enemy;
                }
            }

            Instantiate(goddessSpear, targetEnemy.transform.position, Quaternion.identity);
        }
    }


}
