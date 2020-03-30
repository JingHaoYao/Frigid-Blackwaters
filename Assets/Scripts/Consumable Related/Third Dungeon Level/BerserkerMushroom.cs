using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerMushroom : MonoBehaviour
{
    [SerializeField] ConsumableBonus consumableBonus;
    PlayerScript playerScript;
    [SerializeField] DisplayItem displayItem;
    bool activated = false;

    IEnumerator dealTickDamage()
    {
        for (int i = 0; i < 12; i++)
        {
            PlayerProperties.playerScript.dealTrueDamageToShip(25);
            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        if (consumableBonus.consumableActivated == true && activated == false)
        {
            StartCoroutine(dealTickDamage());
        }
    }
}
