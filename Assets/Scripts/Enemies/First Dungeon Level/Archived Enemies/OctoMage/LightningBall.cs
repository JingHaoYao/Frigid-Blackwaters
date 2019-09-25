using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : MonoBehaviour {
    public GameObject lightning;

    IEnumerator spawnLightning()
    {
        yield return new WaitForSeconds(5f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        GameObject spawnedLightning = Instantiate(lightning, transform.position + new Vector3(0, -2.8f, 0), Quaternion.identity);
        spawnedLightning.GetComponent<ProjectileParent>().instantiater = this.GetComponent<ProjectileParent>().instantiater;
    }

	void Start () {
        Destroy(this.gameObject, 0.75f);
        StartCoroutine(spawnLightning());
	}

	void Update () {
		
	}
}
