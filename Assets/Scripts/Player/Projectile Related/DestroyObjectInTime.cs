using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectInTime: MonoBehaviour {
    public float timeUntilDestroy;
	void Start ()
    {
        Destroy(this.gameObject, timeUntilDestroy);
	}
}
