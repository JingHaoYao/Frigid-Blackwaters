using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnihilationFigure : MonoBehaviour {
    public GameObject explosion;
    public GameObject targetDestroy;
    public Vengeance vengeance;

    void dealDamage()
    {

    }

    void removeFromList()
    {
        vengeance.figureList.Remove(this.gameObject);
    }

    void summonExplosion()
    {
        Instantiate(explosion, transform.position + new Vector3(0, 0.65f, 0), Quaternion.Euler(0, 0, Random.Range(1,360)));
    }

    void Start() {
        Destroy(this.gameObject, 2.583f);
        Invoke("summonExplosion", 16f / 12f);

        if (targetDestroy != null)
        {
            if (targetDestroy.tag == "StrongEnemy")
            {

            }
            else
            {
               Destroy(targetDestroy, 19f / 12f);
            }
        }
        Invoke("removeFromList", 2.483f);
	}

	void Update () {
		
	}
}
