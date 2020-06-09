using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour {

    public GameObject strikeSplash;
    GameObject playerShip;
    public Sprite lightning1, lightning2, lightning3, lightning4;
    SpriteRenderer spriteRenderer;

    void pickSprite()
    {
        int whatSprite = Random.Range(0, 4);
        if (whatSprite == 0)
        {
            spriteRenderer.sprite = lightning1;
        }
        else if (whatSprite == 1)
        {
            spriteRenderer.sprite = lightning2;
        }
        else if (whatSprite == 2)
        {
            spriteRenderer.sprite = lightning3;
        }
        else
        {
            spriteRenderer.sprite = lightning4;
        }
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pickSprite();
        Destroy(this.gameObject, 0.2f);
        playerShip = GameObject.Find("PlayerShip");
        Instantiate(strikeSplash, transform.position + new Vector3(0, -0.2f, 0), Quaternion.identity);
	}

	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(300, this.gameObject);
        }
    }
}
