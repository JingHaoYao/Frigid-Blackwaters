using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistWarningCircle : MonoBehaviour
{
    GameObject playerShip;
    public GameObject smash;
    float timer = 0;

    void spawnSmash()
    {
        GameObject smashInstant = Instantiate(smash, transform.position, Quaternion.identity);
        smashInstant.GetComponent<ProjectileParent>().instantiater = this.GetComponent<ProjectileParent>().instantiater;
        Destroy(this.gameObject);
    }

    void Start()
    {
        playerShip = FindObjectOfType<PlayerScript>().gameObject;
        Invoke("spawnSmash", 2f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer < 1.2)
        {
            transform.position = playerShip.transform.position;
        }
    }
}
