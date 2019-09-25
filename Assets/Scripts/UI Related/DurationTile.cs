using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationTile : MonoBehaviour
{
    public float maxDuration = 0;
    float currDuration = 0;
    public Sprite itemIcon;
    Image fillIndicator;
    
    void Start()
    {
        GetComponent<Image>().sprite = itemIcon;
        fillIndicator = GetComponentsInChildren<Image>()[1];
        currDuration = maxDuration;
    }


    void Update()
    {
        if (currDuration > 0)
        {
            currDuration -= Time.deltaTime;
            fillIndicator.fillAmount = currDuration / maxDuration;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
