using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityStatusEffect : EnemyStatusEffect
{
    [SerializeField] public float slowAmount;

    public override void durationFinishedProcedure()
    {
        targetEnemy.updateSpeed(targetEnemy.speed + slowAmount);
        targetEnemy.removeStatus(this);
        Destroy(this.gameObject);
    }

    private void Start()
    {
        slowDownEnemy();
        StartCoroutine(durationStatus());
    }

    IEnumerator durationStatus()
    {
        yield return new WaitForSeconds(duration);
        durationFinishedProcedure();
    }

    void slowDownEnemy()
    {
        targetEnemy.updateSpeed(targetEnemy.speed + slowAmount);
    }
}
