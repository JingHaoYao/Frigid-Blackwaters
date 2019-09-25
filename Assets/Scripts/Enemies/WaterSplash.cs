using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    public float timeUntilDestroy;
    public float animTime;

    void Start()
    {
        Destroy(this.gameObject, timeUntilDestroy);
        Invoke("disableRenderer", animTime);
    }

    void disableRenderer()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}
