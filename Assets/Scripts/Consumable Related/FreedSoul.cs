using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreedSoul : MonoBehaviour {
    ConsumableBonus consumableBonus;
    bool activated = false;
    public GameObject particles;
    

    void decideWhich()
    {
        int whichBonus = Random.Range(1, 4);
        if(whichBonus == 1)
        {
            consumableBonus.speedBonus = 2;
        }
        else if(whichBonus == 2)
        {
            consumableBonus.attackBonus = 1;
        }
        else
        {
            consumableBonus.restoredHealth = 600;
        }
    }

	void Start () {
        consumableBonus = GetComponent<ConsumableBonus>();
        decideWhich();
	}

    private void Update()
    {
        if (consumableBonus.consumableActivated == true && activated == false)
        {
            float randNum = Random.Range(1.0f, 2.0f);
            Instantiate(particles, GameObject.Find("PlayerShip").transform.position + new Vector3(Mathf.Cos(randNum * Mathf.PI), Mathf.Sin(randNum * Mathf.PI), 0) * 2, Quaternion.identity);
            activated = true;
        }
    }
}
