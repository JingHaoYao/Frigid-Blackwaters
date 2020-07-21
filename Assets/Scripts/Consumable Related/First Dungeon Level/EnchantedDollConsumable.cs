using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantedDollConsumable : MonoBehaviour {
    ConsumableBonus consumableBonus;
    public GameObject doll;
    bool activated = false;

	void Start () {
        consumableBonus = GetComponent<ConsumableBonus>();
        consumableBonus.SetAction(action);
	}

    void action()
    {
        if (activated == false)
        {
            activated = true;
            Instantiate(doll, GameObject.Find("PlayerShip").transform.position, Quaternion.identity);
        }
    }
}
