using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalInvulnerableEffect : MonoBehaviour
{
    public GameObject trackObject;

    public GameObject[] shieldIcons;

    public Sprite[] views;

    public int whichDirectionToAvoid = 0;

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
        foreach (GameObject shield in shieldIcons)
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

        transform.position = trackObject.transform.position;

        for (int i = 0; i < shieldIcons.Length; i++)
        {
            if (i != whichDirectionToAvoid)
            {
                shieldIcons[i].SetActive(true);
                shieldIcons[i].transform.position = trackObject.transform.position + new Vector3(Mathf.Cos((Mathf.PI / 2) * i), Mathf.Sin((Mathf.PI / 2) * i) + 0.4f) * 0.5f;
                shieldIcons[i].GetComponent<SpriteRenderer>().sprite = views[pickView((((Mathf.PI / 2) * i) * Mathf.Rad2Deg + 360) % 360) - 1];
            }
            else
            {
                shieldIcons[i].SetActive(false);
            }
        }
    }
}
