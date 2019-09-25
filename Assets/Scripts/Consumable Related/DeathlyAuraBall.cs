using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathlyAuraBall : MonoBehaviour {
    Animator animator;
    public GameObject deathlyAuraParticles;
    float period = 0;
    bool tick = false;
    GameObject playerShip;

    void destroyBall()
    {
        animator.SetTrigger("FadeOut");
        Destroy(this.gameObject, 0.5f);
    }

	void Start () {
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        Invoke("destroyBall", 30f);
	}

	void Update () {
        transform.position = playerShip.transform.position + new Vector3(0, 2, 0);
        this.GetComponent<SpriteRenderer>().sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
        period += Time.deltaTime;
        if(period > 1)
        {
            tick = true;
            period = 0;
        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "MeleeEnemy" || collision.gameObject.tag == "RangedEnemy" || collision.gameObject.tag == "EnemyShield") && tick == true)
        {
            tick = false;
            GameObject instant = Instantiate(deathlyAuraParticles, transform.position, Quaternion.identity);
            instant.GetComponent<DeathlyAuraParticles>().target = collision.gameObject;
        }
    }
}
