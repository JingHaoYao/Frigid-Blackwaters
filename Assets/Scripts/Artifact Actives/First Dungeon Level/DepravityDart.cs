using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepravityDart : MonoBehaviour
{
    public float angleTravel;
    public float speed = 7;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, angleTravel - 90);
    }

    void Update()
    {
        if (this.GetComponent<Collider2D>().enabled == true)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponent<Animator>().SetTrigger("Dissipate");
        Destroy(this.gameObject, 0.333f);
        this.GetComponent<AudioSource>().Play();
        this.GetComponent<Collider2D>().enabled = false;
    }
}
