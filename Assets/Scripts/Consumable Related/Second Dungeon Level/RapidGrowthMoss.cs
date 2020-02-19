using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidGrowthMoss : MonoBehaviour
{
    ConsumableBonus consumableBonus;
    PlayerScript playerScript;
    [SerializeField] DisplayItem displayItem;
    bool activated = false;

    void Start()
    {
        consumableBonus = GetComponent<ConsumableBonus>();
        playerScript = FindObjectOfType<PlayerScript>();
    }

    IEnumerator heal()
    {
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 11);
        for(int i = 0; i < 11; i++)
        {
            yield return new WaitForSeconds(1f);
            playerScript.healPlayer(100);
        }
        Destroy(this.gameObject);
    }

    void Update()
    {
        if(consumableBonus.consumableActivated == true && activated == false)
        {
            activated = true;
            StartCoroutine(heal());
        }        
    }
}
