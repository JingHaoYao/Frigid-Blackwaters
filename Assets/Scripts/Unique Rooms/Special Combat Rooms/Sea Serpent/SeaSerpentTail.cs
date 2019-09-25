using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentTail : MonoBehaviour {
    Animator animator;
    public SeaSerpentEnemy seaSerpentEnemy;
    public GameObject wave1, wave2, wave3;
    public PolygonCollider2D smashHitBox, swipeHitBox, obstacleHitBox;
    public bool animationCompleted = false;
    int leftRightSwipe = 0;
    SpriteRenderer spriteRenderer;
    GameObject playerShip;

    void done()
    {
        animationCompleted = true;
    }

    public void spawnSwipeWaves()
    {
        int whatAttack = Random.Range(0, 3);
        this.GetComponents<AudioSource>()[2].Play();
        if (whatAttack == 0)
        {
            if (leftRightSwipe == 0)
            {
                GameObject straightWave = Instantiate(wave1, transform.position + new Vector3(-5.78f, -1.87f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

                straightWave = Instantiate(wave1, transform.position + new Vector3(0.76f, -2.86f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

                straightWave = Instantiate(wave1, transform.position + new Vector3(5.73f, 1.04f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
            else
            {
                GameObject straightWave = Instantiate(wave1, transform.position + new Vector3(5.78f, -1.87f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

                straightWave = Instantiate(wave1, transform.position + new Vector3(-0.76f, -2.86f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

                straightWave = Instantiate(wave1, transform.position + new Vector3(-5.73f, 1.04f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else if(whatAttack == 1)
        {
            if (leftRightSwipe == 0)
            {
                GameObject straightWave = Instantiate(wave2, transform.position + new Vector3(-5.78f, -1.87f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 255;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                straightWave = Instantiate(wave2, transform.position + new Vector3(-2.71f, -2.27f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 255;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                straightWave = Instantiate(wave2, transform.position + new Vector3(0.76f, -2.86f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 255;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
            else
            {
                GameObject straightWave = Instantiate(wave2, transform.position + new Vector3(5.78f, -1.87f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 285;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                straightWave.transform.localScale = new Vector3(-0.3f, 0.3f, 0);
                straightWave = Instantiate(wave2, transform.position + new Vector3(2.71f, -2.27f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 285;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                straightWave.transform.localScale = new Vector3(-0.3f, 0.3f, 0);
                straightWave = Instantiate(wave2, transform.position + new Vector3(-0.76f, -2.86f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 285;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                straightWave.transform.localScale = new Vector3(-0.3f, 0.3f, 0);
            }
        }
        else
        {
            if (leftRightSwipe == 0)
            {
                GameObject straightWave = Instantiate(wave3, transform.position + new Vector3(-5.78f, -1.87f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 240;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

                straightWave = Instantiate(wave3, transform.position + new Vector3(0.76f, -2.86f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 240;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

                straightWave = Instantiate(wave3, transform.position + new Vector3(5.73f, 1.04f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 240;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
            else
            {
                GameObject straightWave = Instantiate(wave3, transform.position + new Vector3(5.78f, -1.87f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 300;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                straightWave.transform.localScale = new Vector3(-0.35f, 0.35f, 0);

                straightWave = Instantiate(wave3, transform.position + new Vector3(-0.76f, -2.86f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 300;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                straightWave.transform.localScale = new Vector3(-0.35f, 0.35f, 0);

                straightWave = Instantiate(wave3, transform.position + new Vector3(-5.73f, 1.04f, 0), Quaternion.identity);
                straightWave.GetComponent<SeaSerpentWave>().angleTravel = 300;
                straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                straightWave.transform.localScale = new Vector3(-0.35f, 0.35f, 0);
            }
        }
    }

    public void spawnSmashWaves()
    {
        int whatAttack = Random.Range(0, 3);
        if(whatAttack == 0)
        {
            GameObject straightWave = Instantiate(wave1, transform.position + new Vector3(0, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            straightWave = Instantiate(wave1, transform.position + new Vector3(1.7f, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            straightWave = Instantiate(wave1, transform.position + new Vector3(-1.7f, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        else if(whatAttack == 1)
        {
            GameObject straightWave = Instantiate(wave1, transform.position + new Vector3(0, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 270;
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            straightWave = Instantiate(wave2, transform.position + new Vector3(0, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 240;
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            straightWave = Instantiate(wave2, transform.position + new Vector3(0, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 300;
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            straightWave.transform.localScale = new Vector3(-0.3f, 0.3f, 0);
        }
        else
        {
            GameObject straightWave = Instantiate(wave2, transform.position + new Vector3(0, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 240;
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            straightWave = Instantiate(wave2, transform.position + new Vector3(0, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 300;
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            straightWave.transform.localScale = new Vector3(-0.3f, 0.3f, 0);
            straightWave = Instantiate(wave3, transform.position + new Vector3(0, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 225;
            straightWave = Instantiate(wave3, transform.position + new Vector3(0, -5.27f, 0), Quaternion.identity);
            straightWave.GetComponent<SeaSerpentWave>().angleTravel = 315;
            straightWave.transform.localScale = new Vector3(-0.35f, 0.35f, 0);
            straightWave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
    }

    IEnumerator smashTail()
    {
        animationCompleted = false;
        animator.SetTrigger("Smash");
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(3 / 12f);
        smashHitBox.enabled = true;
        this.GetComponents<AudioSource>()[0].Play();
        spawnSmashWaves();
        yield return new WaitForSeconds(2 / 12f);
        smashHitBox.enabled = false;
        yield return new WaitForSeconds(7 / 12f);
        animationCompleted = true;
    }

    IEnumerator swipeTail()
    {
        leftRightSwipe = Random.Range(0, 2);
        if(leftRightSwipe == 1)
        {
            transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);
        }
        animationCompleted = false;
        animator.SetTrigger("Swipe");
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(4f / 12f);
        swipeHitBox.enabled = true;
        spawnSwipeWaves();
        yield return new WaitForSeconds(2f / 12f);
        swipeHitBox.enabled = false;
        yield return new WaitForSeconds(4f / 12f);
        animationCompleted = true;
    }

    void turnOffObstacleHitBox()
    {
        obstacleHitBox.enabled = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void smash()
    {
        StartCoroutine(smashTail());
    }

    public void swipe()
    {
        StartCoroutine(swipeTail());
    }

    public void submerge()
    {
        animator.SetTrigger("Submerge");
        this.GetComponents<AudioSource>()[0].Play();
        Invoke("turnOffObstacleHitBox", 0.667f);
        Destroy(this.gameObject, 1.7f);
    }

	void Start () {
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("done", 0.667f);
	}

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 16)
        {
            this.GetComponents<AudioSource>()[3].Play();
            seaSerpentEnemy.health--;
            StartCoroutine(hitFrame());
        }
    }
}
