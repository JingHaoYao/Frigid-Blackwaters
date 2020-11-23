using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoothingAlcohol : MonoBehaviour
{
    [SerializeField] ConsumableBonus consumableBonus;

    private void Start()
    {
        consumableBonus.SetAction(() => StartCoroutine(soothingAlcoholRoutine()));
    }

    IEnumerator soothingAlcoholRoutine()
    {
        PlayerProperties.playerScript.healPlayer(1500);
        yield return new WaitForSeconds(15f);
        PlayerProperties.playerScript.healPlayer(2000);
        Destroy(this.gameObject);
    }
}
