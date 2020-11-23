using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoulMist : ArtifactEffect
{
    [SerializeField] GameObject foulMistEffect;
    
    public override void SpawnedEnemy(Enemy enemy)
    {
        GameObject foulMistEffectInstant = Instantiate(foulMistEffect, enemy.transform.position, Quaternion.Euler(270, 0, 0));
        foulMistEffectInstant.GetComponent<EnemyStatusEffect>().targetEnemy = enemy;
    }
}
