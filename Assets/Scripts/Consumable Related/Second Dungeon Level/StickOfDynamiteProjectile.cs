using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickOfDynamiteProjectile : MonoBehaviour
{
    public Vector3 targetPosition;
    public GameObject explosion;
    float speed = 8;

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 720);
        transform.position += (targetPosition - transform.position).normalized * Time.deltaTime * speed;

        if(Vector2.Distance(targetPosition, transform.position) < 0.2f)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
