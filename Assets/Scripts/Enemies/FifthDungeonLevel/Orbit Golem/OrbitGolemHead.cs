using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGolemHead : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] GameObject pyrotheumFollower;
    List<PyrotheumFollower> pyrotheumFollowers = new List<PyrotheumFollower>();
    [SerializeField] GameObject pyrotheumBlast;
    [SerializeField] AudioSource attackAudio;

    [SerializeField] Sprite[] viewSprites;
    private int whatView = 1;
    private int mirror = 1;
    bool isAttacking = false;

    float attackPeriod = 0;
    OrbitGolem golemInstant;
    float anglePeriod = 0;

    public void Initialize(OrbitGolem golem)
    {
        golemInstant = golem;
        SpawnFollowers();
        StartCoroutine(mainLoop());
    }


    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 4;
            mirror = -1;
        }
        else if (angleOrientation >= 180 && angleOrientation < 240)
        {
            whatView = 2;
            mirror = -1;
        }
        else if (angleOrientation >= 240 && angleOrientation < 300)
        {
            whatView = 1;
            mirror = 1;
        }
        else
        {
            whatView = 2;
            mirror = 1;
        }
        transform.localScale = new Vector3(mirror * 4, 4);
    }

    IEnumerator FirePyrotheumProjectile()
    {
        animator.enabled = true;
        isAttacking = true;
        pickView(angleToShip());
        animator.SetTrigger("Attack" + whatView);
        yield return new WaitForSeconds(4 / 12f);

        attackAudio.Play();
        for(int i = 0; i < 4; i++)
        {
            GameObject blastInstant = Instantiate(pyrotheumBlast, transform.position, Quaternion.identity);
            blastInstant.GetComponent<PyrotheumProjectile>().angleTravel = i * 90;
            blastInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }

        yield return new WaitForSeconds(7 / 12f);
        animator.enabled = false;
        isAttacking = false;
    }

    IEnumerator mainLoop()
    {
        while (true)
        {
            anglePeriod += Time.deltaTime * 120;
            if(anglePeriod >= 360)
            {
                anglePeriod = 0;
            }

            transform.position = golemInstant.transform.position + Vector3.up + new Vector3(Mathf.Cos(anglePeriod * Mathf.Deg2Rad), Mathf.Sin(anglePeriod * Mathf.Deg2Rad));

            if(isAttacking == false)
            {
                attackPeriod += Time.deltaTime;

                if (attackPeriod >= 5) {
                    attackPeriod = 0;
                    StartCoroutine(FirePyrotheumProjectile());
                }

                pickView(angleToShip());
                spriteRenderer.sprite = viewSprites[whatView - 1];
            }

            yield return null;
        }
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }


    void SpawnFollowers()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject pyrotheumFollowerInstant = Instantiate(pyrotheumFollower, transform.position, Quaternion.identity);
            pyrotheumFollowerInstant.GetComponent<PyrotheumFollower>().Initialize(this.gameObject, i);
            pyrotheumFollowers.Add(pyrotheumFollowerInstant.GetComponent<PyrotheumFollower>());
        }
    }

    public void InitiateDeath()
    {
        animator.enabled = true;
        StopAllCoroutines();
        spriteRenderer.color = Color.white;
        animator.SetTrigger("Death");

        Destroy(this.gameObject, 0.75f);
        foreach(PyrotheumFollower follower in pyrotheumFollowers)
        {
            follower.dissapear();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            golemInstant.dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }   
    }

    public void FlashHurt()
    {
        StartCoroutine(hitFrame());
    }


    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
