using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DormantGolem : MonoBehaviour {
    Animator animator;
    private bool engaged = false;
    public GameObject challengeGolem;

    IEnumerator spawnGolem()
    {
        animator.SetTrigger("Awaken");
        yield return new WaitForSeconds(0.5f/0.6f);
        Destroy(this.gameObject);
        GameObject instant = Instantiate(challengeGolem, transform.position, Quaternion.identity);
        instant.GetComponent<ChallengeGolem>().anti = this.gameObject.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer;
        //spawn door seals etc etc
    }

	void Start () {
        animator = GetComponent<Animator>();
	}

	void Update () {
		if(Vector2.Distance(this.transform.position, Camera.main.transform.position) < 3)
        {
            if(this.GetComponents<AudioSource>()[0].isPlaying == false)
            {
                this.GetComponents<AudioSource>()[0].Play();
            }
        }
        else
        {
            if(this.GetComponents<AudioSource>()[0].isPlaying == true)
            {
                this.GetComponents<AudioSource>()[0].Stop();
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (engaged == false && collision.gameObject.layer == 16 && Vector2.Distance(Camera.main.transform.position, GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer.gameObject.transform.position) < 3)
        {
            this.GetComponents<AudioSource>()[1].Play();
            this.gameObject.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer.spawnDoorSeals();
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = false;
            engaged = true;
            StartCoroutine(spawnGolem());
        }
    }
}
