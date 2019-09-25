using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRendererLayer : MonoBehaviour {
    SpriteRenderer rend;
    public float offset = 0;
    public int rendLayerOffset = 0;

    void pickRendererLayer()
    {
        rend.sortingOrder = (200 - (int)((transform.position.y + offset) * 10)) + rendLayerOffset;
    }

    void Start () {
        rend = GetComponent<SpriteRenderer>();
        pickRendererLayer();
    }

	void Update () {
        pickRendererLayer();
	}
}
