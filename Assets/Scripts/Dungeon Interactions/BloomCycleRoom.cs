using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomCycleRoom : RoomInteraction
{

    IEnumerator BeginBloomProcess(float durationUntilBloom)
    {
        float increment = durationUntilBloom / 3;
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(durationUntilBloom);
            UpdateBloomObstacles(i + 1);
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
            StartCoroutine(BeginBloomProcess(20 - (dangerValue / 10 * 20)));
        }
    }

    public override void RoomFinished()
    {
        StopAllCoroutines();    
    }
}
