using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxScript : MonoBehaviour {
    public static bool playerShielded = false;
    PlayerScript playerScript;
    SpriteRenderer playerShipSR;
    PolygonCollider2D polygonCollider;

    void pickHitBox()
    {
        if (this.gameObject.name == "DiagonalDownHitBox")
        {
            if ((playerScript.angleOrientation > 195 && playerScript.angleOrientation <= 255) || (playerScript.angleOrientation > 285 && playerScript.angleOrientation <= 345))
            {
                polygonCollider.enabled = true;
            }
            else
            {
                polygonCollider.enabled = false;
            }
        }
        else if (this.gameObject.name == "LeftHitBox")
        {
            if ((playerScript.angleOrientation > 165 && playerScript.angleOrientation <= 195) || (playerScript.angleOrientation > 345) || (playerScript.angleOrientation <= 15))
            {
                polygonCollider.enabled = true;
            }
            else
            {
                polygonCollider.enabled = false;
            }
        }
        else if (this.gameObject.name == "DiagonalUpHitBox")
        {
            if ((playerScript.angleOrientation > 15 && playerScript.angleOrientation <= 75) || (playerScript.angleOrientation > 105 && playerScript.angleOrientation <= 165))
            {
                polygonCollider.enabled = true;
            }
            else
            {
                polygonCollider.enabled = false;
            }
        }
        else if (this.gameObject.name == "UpHitBox")
        {
            if (playerScript.angleOrientation > 75 && playerScript.angleOrientation <= 105)
            {
                polygonCollider.enabled = true;
            }
            else
            {
                polygonCollider.enabled = false;
            }
        }
        else
        {
            if (playerScript.angleOrientation > 255 && playerScript.angleOrientation <= 285)
            {
                polygonCollider.enabled = true;
            }
            else
            {
                polygonCollider.enabled = false;
            }
        }
    }

	void Start () {
        playerScript = transform.parent.gameObject.GetComponent<PlayerScript>();
        polygonCollider = this.gameObject.GetComponent<PolygonCollider2D>();
	}
	
	void Update () {
        pickHitBox();
	}
}
