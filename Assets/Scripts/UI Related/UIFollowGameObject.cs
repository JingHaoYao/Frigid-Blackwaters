using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowGameObject : MonoBehaviour
{
    public GameObject followObject;
    public Vector3 offset;
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(followObject.transform.position + offset);
    }
}
