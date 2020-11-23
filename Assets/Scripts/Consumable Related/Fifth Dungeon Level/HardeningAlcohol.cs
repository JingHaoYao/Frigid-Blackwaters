using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardeningAlcohol : MonoBehaviour
{
    [SerializeField] ConsumableBonus consumableBonus;

    private void Start()
    {
        consumableBonus.SetAction(GrantConsumableBonus);
    }

    void GrantConsumableBonus()
    {
        StartCoroutine(grantBonuses());
    }

    IEnumerator grantBonuses()
    {
        PlayerProperties.playerScript.conHealthBonus += 3000;
        PlayerProperties.playerScript.CheckAndUpdateHealth();
        PlayerProperties.playerScript.healPlayer(3000);

        yield return new WaitForSeconds(60f);

        PlayerProperties.playerScript.conHealthBonus -= 3000;
        PlayerProperties.playerScript.CheckAndUpdateHealth();

        Destroy(this.gameObject);
    }
}
