using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInvulnerableEffect : MonoBehaviour
{
    public GameObject trackObject;

    public GameObject[] shieldIcons;

    public Sprite[] views;
    public float trackingAngle = 0;

    int pickView(float angle)
    {
        if (angle > 255 && angle <= 285)
        {
            return 1;
        }
        else if (angle > 285 && angle <= 360)
        {
            return 2;
        }
        else if (angle > 180 && angle <= 255)
        {
            return 6;
        }
        else if (angle > 75 && angle <= 105)
        {
            return 4;
        }
        else if (angle >= 0 && angle <= 75)
        {
            return 3;
        }
        else
        {
            return 5;
        }
    }

    private void Start()
    {
        foreach(GameObject shield in shieldIcons)
        {
            shield.transform.localScale = new Vector3((1 / trackObject.transform.localScale.x) * 1.7f, (1 / trackObject.transform.localScale.y) * 1.7f);
        }
    }

    void Update()
    {
        if (trackObject == null)
        {
            Destroy(this.gameObject);
        }

        trackingAngle += Time.deltaTime * 1.5f;
        if (trackingAngle >= Mathf.PI * 2)
        {
            trackingAngle = 0;
        }

        transform.position = trackObject.transform.position;

        for (int i = 0; i < 3; i++)
        {
            shieldIcons[i].transform.position = trackObject.transform.position + new Vector3(Mathf.Cos(trackingAngle + ((Mathf.PI * 2) / 3) * i), Mathf.Sin(trackingAngle + ((Mathf.PI * 2) / 3) * i) + 0.4f) * 0.5f;
            shieldIcons[i].GetComponent<SpriteRenderer>().sprite = views[pickView(((trackingAngle + ((Mathf.PI * 2) / 3) * i) * Mathf.Rad2Deg + 360) % 360) - 1];
        }
    }
}
