using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRendererLayerParticles : MonoBehaviour {
    ParticleSystemRenderer partSys;

	void Start () {
        partSys = GetComponent<ParticleSystemRenderer>();
	}

	void Update () {
		partSys.sortingOrder = 200 - (int)(transform.position.y * 10);
    }
}
