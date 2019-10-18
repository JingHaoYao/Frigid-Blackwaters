using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject objectToFollow;

    void Update()
    {
        if (objectToFollow == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.position = objectToFollow.transform.position;
        }
    }
}
