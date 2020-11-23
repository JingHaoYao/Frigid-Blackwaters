using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisStopwatch : MonoBehaviour
{
    int damageToRevert = 0;
    int prevTrueDamage = 0;
    ConsumableBonus consumableBonus;
    PlayerScript playerScript;
    public GameObject stopWatchEffect;
    bool activated = false;

    IEnumerator delayStasisPeriod(int damage)
    {
        yield return new WaitForSeconds(5f);
        damageToRevert = damage;
    }

    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        consumableBonus = GetComponent<ConsumableBonus>();
        consumableBonus.SetAction(startStopWatch);
    }

    void Update()
    {
        if(prevTrueDamage != playerScript.shipHealth)
        {
            prevTrueDamage = playerScript.shipHealth;
            StartCoroutine(delayStasisPeriod(playerScript.shipHealth));
        }
    }

    void startStopWatch()
    {
        if (activated == false)
        {
            activated = true;
            GameObject instant = Instantiate(stopWatchEffect, playerScript.transform.position, Quaternion.identity);
            instant.GetComponent<FollowObject>().objectToFollow = playerScript.gameObject;
            playerScript.shipHealth = damageToRevert;
            Destroy(this.gameObject);
        }
    }
}
