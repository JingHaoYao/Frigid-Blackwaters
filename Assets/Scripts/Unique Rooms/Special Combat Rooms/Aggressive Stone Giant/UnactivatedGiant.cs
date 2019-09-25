using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnactivatedGiant : MonoBehaviour {
    public bool engaged = false;
    public GameObject giantHeadEnter;
    Animator[] fistAnimators;

    public void spawnEnemy()
    {
        StartCoroutine(spawnGiant());
    }

    IEnumerator spawnGiant()
    {
        this.gameObject.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer.spawnDoorSeals();
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = false;
        foreach (Animator element in fistAnimators)
        {
            element.SetTrigger("Sink");
            this.GetComponent<AudioSource>().Play();
            Destroy(element.gameObject, 7f / 12f);
        }
        yield return new WaitForSeconds(7f / 12f);
        GameObject instant = Instantiate(giantHeadEnter, transform.position, Quaternion.identity);
        instant.GetComponent<StoneGiantHeadRiseIn>().anti = this.gameObject.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer;
        yield return new WaitForSeconds(17f / 12f);
        Destroy(this.gameObject);
        //spawn door seals etc etc
    }

    void Start()
    {
        fistAnimators = GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        
    }
}
