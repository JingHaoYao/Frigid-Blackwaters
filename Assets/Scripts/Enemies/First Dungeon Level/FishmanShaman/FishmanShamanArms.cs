using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishmanShamanArms : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    CircleCollider2D circCol;
    Animator animator;
    bool collidedwithShip = false, isRooted = false, hasHit = false;
    GameObject playerShip;
    public GameObject waterCircles, splashCircles;

    void pickRendererLayer()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) > 1)
        {
            spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
        }
        else
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 4;
        }
    }

    IEnumerator grab()
    {
        yield return new WaitForSeconds(1f / 12f);
        GameObject waterCirc = Instantiate(waterCircles, transform.position, Quaternion.identity);
        waterCirc.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) - 2;
        yield return new WaitForSeconds(4f / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(1f/12f);
        if(collidedwithShip == true)
        {
            isRooted = true;
            yield return new WaitForSeconds(2f);
            animator.SetTrigger("ArmsOut");
            isRooted = false;
            PlayerProperties.playerScript.removeRootingObject();
            yield return new WaitForSeconds(6f / 12f);
            GameObject splashCirc = Instantiate(splashCircles, transform.position, Quaternion.identity);
            splashCirc.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) - 2;
            yield return new WaitForSeconds(1f / 12f);
            Destroy(this.gameObject);
        }
        else
        {
            circCol.enabled = false;
            animator.SetTrigger("ArmsOut");
            yield return new WaitForSeconds(6f / 12f);
            GameObject splashCirc = Instantiate(splashCircles, transform.position, Quaternion.identity);
            splashCirc.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) - 2;
            yield return new WaitForSeconds(1f / 12f);
            Destroy(this.gameObject);
        }
    }

	void Start () {
        circCol = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        StartCoroutine(grab());
	}

	void Update () {
		if(isRooted == true)
        {
            PlayerProperties.playerScript.addRootingObject();
        }
        pickRendererLayer();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox" && hasHit == false)
        {
            collidedwithShip = true;
            PlayerProperties.playerScript.dealDamageToShip(150, this.gameObject);
            circCol.enabled = false;
            hasHit = true;
        }
    }
}
