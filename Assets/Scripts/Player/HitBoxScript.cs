
using UnityEngine;
using UnityEngine.Events;

public class HitBoxScript : MonoBehaviour {
    public static bool playerShielded = false;
    PlayerScript playerScript;
    SpriteRenderer playerShipSR;
    PolygonCollider2D polygonCollider;

    UnityAction hitBoxAction;

    void pickHitBox()
    {
        if (this.gameObject.name == "DiagonalDownHitBox")
        {
            hitBoxAction = () => { polygonCollider.enabled = (playerScript.angleOrientation > 195 && playerScript.angleOrientation <= 255) || (playerScript.angleOrientation > 285 && playerScript.angleOrientation <= 345); };
        }
        else if (this.gameObject.name == "LeftHitBox")
        {
            hitBoxAction = () => { polygonCollider.enabled = (playerScript.angleOrientation > 165 && playerScript.angleOrientation <= 195) || (playerScript.angleOrientation > 345) || (playerScript.angleOrientation <= 15); };
        }
        else if (this.gameObject.name == "DiagonalUpHitBox")
        {
            hitBoxAction = () => { polygonCollider.enabled = (playerScript.angleOrientation > 15 && playerScript.angleOrientation <= 75) || (playerScript.angleOrientation > 105 && playerScript.angleOrientation <= 165); };
        }
        else if (this.gameObject.name == "UpHitBox")
        {
            hitBoxAction = () => { polygonCollider.enabled = playerScript.angleOrientation > 75 && playerScript.angleOrientation <= 105; };
        }
        else
        {
            hitBoxAction = () => { polygonCollider.enabled = playerScript.angleOrientation > 255 && playerScript.angleOrientation <= 285; };
        }
    }

	void Start () {
        playerScript = transform.parent.gameObject.GetComponent<PlayerScript>();
        polygonCollider = this.gameObject.GetComponent<PolygonCollider2D>();
        pickHitBox();
	}
	
	void Update () {
        hitBoxAction();
	}
}
