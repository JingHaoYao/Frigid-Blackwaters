using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRendererLayer : MonoBehaviour {
    SpriteRenderer rend;
    public float offset = 0;
    public int rendLayerOffset = 0;
    public bool updateOnce = false;

    void pickRendererLayer()
    {
        rend.sortingOrder = (200 - (int)((transform.position.y + offset) * 10)) + rendLayerOffset;
    }

    void OnEnable() {
        rend = GetComponent<SpriteRenderer>();
        pickRendererLayer();
        if(updateOnce == false)
        {
            StartCoroutine(updateConstantly());
        }
    }

    IEnumerator updateConstantly()
    {
        while (true)
        {
            pickRendererLayer();
            yield return null;
        }
    }
}
