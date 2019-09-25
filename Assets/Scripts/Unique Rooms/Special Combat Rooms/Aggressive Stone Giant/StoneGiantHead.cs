using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGiantHead : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    public Sprite facingDown, facingDownLeft, facingLeft, facingUpLeft, facingUp;
    float angleToShip;
    GameObject playerShip;
    public GameObject baseHead;

    void pickSprite(float angleOrientation)
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            spriteRenderer.sprite = facingUpLeft;
            transform.localScale = new Vector3(-0.7f, 0.7f, 0);
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            spriteRenderer.sprite = facingUp;
            transform.localScale = new Vector3(0.7f, 0.7f, 0);
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            spriteRenderer.sprite = facingUpLeft;
            transform.localScale = new Vector3(0.7f, 0.7f, 0);
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(0.7f, 0.7f, 0);
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            spriteRenderer.sprite = facingDownLeft;
            transform.localScale = new Vector3(0.7f, 0.7f, 0);
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {

            spriteRenderer.sprite = facingDown;
            transform.localScale = new Vector3(0.7f, 0.7f, 0);
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            spriteRenderer.sprite = facingDownLeft;
            transform.localScale = new Vector3(-0.7f, 0.7f, 0);
        }
        else
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(-0.7f, 0.7f, 0);
        }
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
	}

	void Update () {
        angleToShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;
        pickSprite(angleToShip);
        spriteRenderer.sortingOrder = baseHead.GetComponent<SpriteRenderer>().sortingOrder + 1;
	}
}
