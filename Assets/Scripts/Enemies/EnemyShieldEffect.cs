using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldEffect : MonoBehaviour
{
    public GameObject trackObject;
    public Sprite[] views;
    public float trackingAngle = 0;

    int pickView()
    {
        float angle = ((trackingAngle * Mathf.Rad2Deg) + 360) % 360;
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
        transform.position = trackObject.transform.position + Vector3.up * 0.7f + new Vector3(Mathf.Cos(trackingAngle), Mathf.Sin(trackingAngle)) * 0.8f;
        this.GetComponent<SpriteRenderer>().sprite = views[pickView() - 1];
    }
}
