using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBossHead : MonoBehaviour
{
    float spinPeriod = 0;
    SpriteRenderer spriteRenderer;
    PlayerScript playerScript;

    public Sprite[] viewList;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerScript = FindObjectOfType<PlayerScript>();
    }

    int pickView(float angle)
    {
        if (angle > 22.5f && angle <= 67.5f)
        {
            return 5;
        }
        else if (angle > 67.5f && angle <= 112.5)
        {
            return 4;
        }
        else if (angle > 112.5 && angle <= 157.5)
        {
            return 3;
        }
        else if (angle > 157.5 && angle <= 202.5)
        {
            return 2;
        }
        else if (angle > 202.5f && angle <= 247.5f)
        {
            return 1;
        }
        else if (angle > 247.5f && angle <= 292.5f)
        {
            return 0;
        }
        else if(angle > 292.5f && angle < 337.5)
        {
            return 7;
        }
        else
        {
            return 6;
        }
    }

    private void Update()
    {
        spriteRenderer.sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder;
        spinPeriod += Time.deltaTime * 6;

        float angleToShip = (Mathf.Atan2(playerScript.transform.position.y - transform.parent.position.y, playerScript.transform.position.x - transform.parent.position.x) * Mathf.Rad2Deg + 360) % 360;
        spriteRenderer.sprite = viewList[pickView(angleToShip)];

        if (spinPeriod > 2 * Mathf.PI)
        {
            spinPeriod = 0;
        }

        transform.position = transform.parent.position + new Vector3(0, 4.13f, 0) + new Vector3(Mathf.Cos(spinPeriod) * 0.6f, Mathf.Sin(spinPeriod) * 0.3f);
    }
}
