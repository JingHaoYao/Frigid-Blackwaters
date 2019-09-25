using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSymbol : MonoBehaviour
{
    public GameObject map;
    public bool dungeonMap = false;

    void Update()
    {
        if (dungeonMap == false)
        {
            if (map.activeSelf == true)
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            if(map.transform.localScale != new Vector3(0, 0, 0))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
