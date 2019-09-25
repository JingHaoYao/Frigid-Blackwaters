using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentBubbles : MonoBehaviour {
    public GameObject seaSerpentEnemy;
    WhichRoomManager whichRoomManager;
    public GameObject seaSerpentBite;
    SpriteRenderer spriteRenderer;
    private bool activated = false;

    IEnumerator spawnEnemy()
    {
        GameObject instant = Instantiate(seaSerpentEnemy, transform.position, Quaternion.identity);
        instant.GetComponent<SeaSerpentEnemy>().initBubbles = this.gameObject;
        instant.GetComponent<SeaSerpentEnemy>().anti = whichRoomManager.antiSpawnSpaceDetailer;
        whichRoomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        yield return new WaitForSeconds(1.5f);
        spriteRenderer.enabled = false;
        GameObject bite = Instantiate(seaSerpentBite, transform.position + new Vector3(-0.2f, -0.1f, 0), Quaternion.identity);
        bite.GetComponent<SeaSerpentEmergeAttack>().seaSerpentEnemy = instant.GetComponent<SeaSerpentEnemy>();
        yield return new WaitForSeconds(1.750f / 1.5f);
        Destroy(this.gameObject);
    }

	void Start () {
        whichRoomManager = GetComponent<WhichRoomManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox" && activated == false)
        {
            activated = true;
            StartCoroutine(spawnEnemy());
            //Debug.Log("Player Activated");
        }
    }
}
