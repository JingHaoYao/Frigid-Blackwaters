using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmorEffect : MonoBehaviour
{
    public GameObject trackObject;

    public GameObject[] shieldIcons;

    public Sprite[] views;
    public Sprite[] brokenViews;
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

    public void updateShieldEffect()
    {
        if (trackObject.GetComponent<PlayerScript>().defenseModifier < 1 && trackObject.GetComponent<PlayerScript>().defenseModifier > 0.75f)
        {
            shieldIcons[0].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[1].GetComponent<SpriteRenderer>().enabled = false;
            shieldIcons[2].GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (trackObject.GetComponent<PlayerScript>().defenseModifier <= 0.75f && trackObject.GetComponent<PlayerScript>().defenseModifier > 0.5f)
        {
            shieldIcons[0].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[1].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[2].GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (trackObject.GetComponent<PlayerScript>().defenseModifier <= 0.25f)
        {
            shieldIcons[0].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[1].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[2].GetComponent<SpriteRenderer>().enabled = true;
        }
        else if (trackObject.GetComponent<PlayerScript>().defenseModifier > 1 && trackObject.GetComponent<PlayerScript>().defenseModifier < 1.25f)
        {
            shieldIcons[0].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[1].GetComponent<SpriteRenderer>().enabled = false;
            shieldIcons[2].GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (trackObject.GetComponent<PlayerScript>().defenseModifier >= 1.25f && trackObject.GetComponent<PlayerScript>().defenseModifier < 1.5f)
        {
            shieldIcons[0].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[1].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[2].GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (trackObject.GetComponent<PlayerScript>().defenseModifier >= 1.5f)
        {
            shieldIcons[0].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[1].GetComponent<SpriteRenderer>().enabled = true;
            shieldIcons[2].GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            shieldIcons[0].GetComponent<SpriteRenderer>().enabled = false;
            shieldIcons[1].GetComponent<SpriteRenderer>().enabled = false;
            shieldIcons[2].GetComponent<SpriteRenderer>().enabled = false;
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
            shieldIcons[i].transform.position = trackObject.transform.position + new Vector3(Mathf.Cos(trackingAngle + ((Mathf.PI * 2) / 3) * i), Mathf.Sin(trackingAngle + ((Mathf.PI * 2) / 3) * i)) * 0.9f;
            if (trackObject.GetComponent<PlayerScript>().defenseModifier < 1)
            {
                shieldIcons[i].GetComponent<SpriteRenderer>().sprite = views[pickView(((trackingAngle + ((Mathf.PI * 2) / 3) * i) * Mathf.Rad2Deg + 360) % 360) - 1];
            }
            else
            {
                shieldIcons[i].GetComponent<SpriteRenderer>().sprite = brokenViews[pickView(((trackingAngle + ((Mathf.PI * 2) / 3) * i) * Mathf.Rad2Deg + 360) % 360) - 1];
            }
        }
    }
}
