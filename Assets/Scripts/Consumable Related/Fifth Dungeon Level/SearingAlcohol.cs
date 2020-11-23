using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearingAlcohol : MonoBehaviour
{
    [SerializeField] ConsumableBonus consumableBonus;

    private void Start()
    {
        consumableBonus.SetAction(StartDamageRoutine);
    }

    IEnumerator damageRoutine()
    {
        for(int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(1f);
            PlayerProperties.playerScript.dealTrueDamageToShip(100);
        }

        Destroy(this.gameObject);
    }

    void StartDamageRoutine()
    {
        StartCoroutine(damageRoutine());
    }
}
