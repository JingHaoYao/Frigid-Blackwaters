using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGemMissile : MonoBehaviour
{
    public float angleTravel;
    public float speed = 10;
    float animDonePeriod = 0;

    void Update()
    {
        if(animDonePeriod < 0.5f)
        {
            animDonePeriod += Time.deltaTime;
        }

        if (this.GetComponent<Collider2D>().enabled == true && animDonePeriod >= 0.5f)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponent<Animator>().SetTrigger("Explode");
        Destroy(this.gameObject, 0.5f);
        this.GetComponent<AudioSource>().Play();
        this.GetComponent<Collider2D>().enabled = false;
    }
}
