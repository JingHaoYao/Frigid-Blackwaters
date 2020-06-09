using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaguePuddle : MonoBehaviour {
    BoxCollider2D boxCol;
    GameObject playerShip;
    Animator animator;
    public GameObject smokeParticles;
    private bool smoke = false;
    private float smokeSpawnPeriod = 0;

	void Start () {
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.enabled = false;
        playerShip = GameObject.Find("PlayerShip");
        animator = GetComponent<Animator>();
        StartCoroutine(puddleAnim(8));
	}

    IEnumerator puddleAnim(float stagnantDuration)
    {
        yield return new WaitForSeconds(4f / 12f);
        smoke = true;
        animator.SetTrigger("Stagnant");
        boxCol.enabled = true;
        yield return new WaitForSeconds(stagnantDuration);
        boxCol.enabled = false;
        animator.SetTrigger("SpreadOut");
        smoke = false;
        yield return new WaitForSeconds(4f / 12f);
        Destroy(this.gameObject);
    }

    void Update () {
		if(smoke == true)
        {
            smokeSpawnPeriod += Time.deltaTime;
            if(smokeSpawnPeriod >= 0.1f)
            {
                smokeSpawnPeriod = 0;
                Instantiate(smokeParticles, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 0.5f), 0), Quaternion.identity);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(500, this.gameObject);
        }
    }
}
