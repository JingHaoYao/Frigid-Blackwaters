using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestructiveMushroom : MonoBehaviour
{
    [SerializeField] ConsumableBonus consumableBonus;
    PlayerScript playerScript;
    [SerializeField] DisplayItem displayItem;
    bool activated = false;

    private void Start()
    {
        consumableBonus.SetAction(damageAndHeal);
    }

    IEnumerator heal()
    {
        yield return new WaitForSeconds(5f);
        PlayerProperties.playerScript.healPlayer(1750);
        Destroy(this.gameObject);
    }

    void damageAndHeal()
    {
        PlayerProperties.playerScript.dealTrueDamageToShip(500);
        StartCoroutine(heal());
    }
}
