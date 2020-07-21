using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutingSeedPod : MonoBehaviour
{
    [SerializeField] ConsumableBonus consumableBonus;
    PlayerScript playerScript;
    [SerializeField] DisplayItem displayItem;
    bool activated = false;

    IEnumerator heal()
    {
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 15);
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(1f);
            PlayerProperties.playerScript.healPlayer(100);
        }
        Destroy(this.gameObject);
    }

    private void Start()
    {
        consumableBonus.SetAction(activateHeal);
    }

    void activateHeal()
    {
        if (activated == false)
        {
            activated = true;
            StartCoroutine(heal());
        }
    }
}
