using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashedMedicalShip : MonoBehaviour {
    public GameObject medicalCrate, medicalBarrel;

    void spawnMedicalSupplies()
    {
        for(int i = 0; i < 3; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(transform.position.x - 8, transform.position.x + 8), Random.Range(transform.position.y - 8, transform.position.y + 8), 0);
            while(Physics2D.OverlapCircle(randPos, 1.5f) == true)
            {
                randPos = new Vector3(Random.Range(transform.position.x - 8, transform.position.x + 8), Random.Range(transform.position.y - 8, transform.position.y + 8), 0);
            }
            GameObject spawnedObj;
            if (Random.Range(0, 2) == 1)
            {
                spawnedObj = Instantiate(medicalCrate, randPos, Quaternion.identity);
            }
            else
            {
                spawnedObj = Instantiate(medicalBarrel, randPos, Quaternion.identity);
            }
            
            if(Random.Range(0,2) == 1)
            {
                spawnedObj.transform.localScale = new Vector3(-0.3f, 0.3f, 0);
            }
        }
    }

	void Start () {
        spawnMedicalSupplies();
	}
}
