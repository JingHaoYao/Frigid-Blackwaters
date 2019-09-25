using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelScatterRockProjectile : MonoBehaviour
{
    public float angleToTravel;
    public float travelSpeed = 8;
    bool collided = false;

    private void Start()
    {
        if (Random.Range(0, 2) == 1)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void Update()
    {
        if (collided == false)
        {
            transform.position += Time.deltaTime * travelSpeed * new Vector3(Mathf.Cos(angleToTravel * Mathf.Deg2Rad), Mathf.Sin(angleToTravel * Mathf.Deg2Rad));
        }
    }

    public void breakRock()
    {
        collided = true;
        transform.GetChild(0).gameObject.SetActive(false);
        this.GetComponent<AudioSource>().Play();
        this.GetComponent<PolygonCollider2D>().enabled = false;
        this.GetComponent<Animator>().SetTrigger("Break");
        Destroy(this.gameObject, 0.417f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        breakRock();
    }
}
