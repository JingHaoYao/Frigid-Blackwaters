using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderKeg : MonoBehaviour
{
    ConsumableBonus consumableBonus;
    PlayerScript playerScript;
    public GameObject keg;
    bool activated = false;

    void Start()
    {
        consumableBonus = GetComponent<ConsumableBonus>();
        playerScript = FindObjectOfType<PlayerScript>();
        consumableBonus.SetAction(activate);
    }

    void activate()
    {
        if (activated == false)
        {
            activated = true;
            Instantiate(keg, playerScript.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
