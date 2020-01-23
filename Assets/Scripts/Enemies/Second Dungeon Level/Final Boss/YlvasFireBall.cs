using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YlvasFireBall : MonoBehaviour
{
    public float angleTravel;
    [SerializeField] private Rigidbody2D rigidBody2D;
    public GameObject impactObject;

    void Start()
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * 8;
        transform.rotation = Quaternion.Euler(0, 0, 90 + angleTravel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(impactObject, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
