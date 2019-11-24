using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBossHead : MonoBehaviour
{
    float spinPeriod = 0;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        spriteRenderer.sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder;
        spinPeriod += Time.deltaTime * 6;

        if (spinPeriod > 2 * Mathf.PI)
        {
            spinPeriod = 0;
        }

        transform.position = transform.parent.position + new Vector3(0, 4.13f, 0) + new Vector3(Mathf.Cos(spinPeriod) * 0.6f, Mathf.Sin(spinPeriod) * 0.3f);
    }
}
