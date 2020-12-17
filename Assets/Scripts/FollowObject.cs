using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject objectToFollow;
    [SerializeField] Vector3 offset;

    void Update()
    {
        if (objectToFollow == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.position = objectToFollow.transform.position + offset;
        }
    }
}
