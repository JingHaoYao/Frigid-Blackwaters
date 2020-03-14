using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeShield : MonoBehaviour
{
    public GameObject blackholeEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 13 && collision.gameObject.GetComponent<ProjectileParent>())
        {
            Destroy(collision.gameObject);
            Instantiate(blackholeEffect, collision.ClosestPoint(collision.gameObject.transform.position), Quaternion.identity);
        }
    }
}
