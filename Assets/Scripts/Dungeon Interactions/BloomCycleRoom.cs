using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomCycleRoom : RoomInteraction
{
    public GameObject bloomStatusEffect;

    IEnumerator BeginBloomProcess(float durationUntilBloom)
    {
        float increment = durationUntilBloom / 3;
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(increment);
            UpdateBloomObstacles(i + 1);
        }
        applyBloomStatusEffects();
    }

    void applyBloomStatusEffects()
    {
        foreach (Enemy enemy in EnemyPool.enemyPool)
        {
            GameObject effectInstant = Instantiate(bloomStatusEffect, enemy.transform.position, Quaternion.identity);
            enemy.addStatus(effectInstant.GetComponent<BloomStatusEffect>());
        }
    }

    void UpdateBloomObstacles(int bloomProgress)
    {
        foreach(GameObject solidObstacle in this.allSpawnedObstacles)
        {
            solidObstacle.GetComponent<BloomObstacle>().MoveToNextBloomState(bloomProgress);
        }
    }

    public override void RoomInitialized(int dangerValue)
    {
        if (!EnemyPool.isPoolEmpty() && this.allSpawnedObstacles.Count > 0)
        {
            StartCoroutine(BeginBloomProcess(30 - (dangerValue / 10 * 20)));
        }
    }

    public override void RoomFinished()
    {
        StopAllCoroutines();    
    }
}
