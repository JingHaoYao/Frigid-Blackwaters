using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossEnergyBall : MonoBehaviour
{
    public float angleTravel;
    Animator animator;
    bool impacted = false;
    GameObject playerShip;
    public int travelSpeed = 10;
    float dissipateTimer = 0;
    public bool isSmallBall = false;
    public GameObject smallBall;
    float particleTimer = 0;
    public GameObject particles;

    IEnumerator dissipate()
    {
        animator.SetTrigger("Dissipate");
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(0.583f - 0.2f);
        Destroy(this.gameObject);
        int offSet = Random.Range(15, 61);
        for(int i = 0; i < 8; i++)
        {
            GameObject ball = Instantiate(smallBall, transform.position, Quaternion.identity);
            ball.GetComponent<FirstBossEnergyBall>().angleTravel = offSet + i * 45;
            ball.GetComponent<ProjectileParent>().instantiater = this.GetComponent<ProjectileParent>().instantiater;
        }
        //summon things
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        transform.rotation = Quaternion.Euler(0, 0, angleTravel);
    }

    void Update()
    {
        if (isSmallBall == false)
        {
            if (dissipateTimer < 4)
            {
                dissipateTimer += Time.deltaTime;

                if (impacted == false)
                {
                    transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * travelSpeed;
                }
            }
            else
            {
                if (impacted == false)
                {
                    impacted = true;
                    StartCoroutine(dissipate());
                }
            }
        }
        else
        {
            if (impacted == false)
            {
                transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * travelSpeed;
            }

            particleTimer += Time.deltaTime;
            if (particleTimer >= 0.05 && impacted == false)
            {
                GameObject instantParticles = Instantiate(particles, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
                particleTimer = 0;
            }
        }

        if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 9 || Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > 9)
        {
            if (impacted == false)
            {
                impacted = true;
                animator.SetTrigger("Impact");
                this.GetComponents<AudioSource>()[0].Play();
                Destroy(this.gameObject, 0.5f / 2f);
                this.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            if (isSmallBall == false)
            {
                PlayerProperties.playerScript.dealDamageToShip(400, this.gameObject);
            }
            else
            {
                PlayerProperties.playerScript.dealDamageToShip(200, this.gameObject);
            }
        }
    }
}
