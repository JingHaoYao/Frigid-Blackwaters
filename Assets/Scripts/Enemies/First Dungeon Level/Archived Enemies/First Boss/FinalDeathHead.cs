using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDeathHead : MonoBehaviour
{
    public GameObject splash;
    Vector3 currentPosition;

    void Start()
    {
        currentPosition = transform.position;
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, currentPosition + new Vector3(0, -6.15f, 0f)) < 0.2f || transform.position.y < (currentPosition + new Vector3(0, -6.15f, 0)).y){
            Instantiate(splash, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
