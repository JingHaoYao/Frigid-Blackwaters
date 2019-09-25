using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearIndicator : MonoBehaviour {
    public GameObject downWardsSpear;
    public float waitDuration = 0.5f;

    void endIndicator()
    {
        GameObject spear = Instantiate(downWardsSpear, transform.position + new Vector3(0, 15, 0), Quaternion.identity);
        spear.GetComponent<FirstbossDownwardsSpear>().target = transform.position;
        Destroy(this.gameObject);
    }

	void Start () {
        Invoke("endIndicator", waitDuration);	
	}
}
