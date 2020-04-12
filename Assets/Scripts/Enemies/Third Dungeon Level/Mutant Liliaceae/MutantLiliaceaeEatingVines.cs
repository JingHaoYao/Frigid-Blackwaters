using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantLiliaceaeEatingVines : MonoBehaviour
{
    [SerializeField] GameObject waterSplash;
    public GameObject targetEnemy;
    public MutantLiliaceae boss;
    bool destroyedEnemy = false;

    private void Start()
    {
        StartCoroutine(startProcedure());
    }

    IEnumerator followTargetEnemy()
    {
        while (!destroyedEnemy)
        {
            transform.position = targetEnemy.transform.position;
            yield return null;
        }
    }

    IEnumerator startProcedure()
    {
        StartCoroutine(followTargetEnemy());
        yield return new WaitForSeconds(10 / 12f);
        destroyedEnemy = true;
        StopCoroutine(followTargetEnemy());
        Destroy(targetEnemy);
        boss.addConsumedJumper();
        Instantiate(waterSplash, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(6 / 12f);
        Destroy(this.gameObject);
    }
}
