using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YlvaCompanion : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource fireAudio;
    public GameObject fireBall;
    public PlayerScript playerScript;
    public CursorTarget cursorTarget;
    private float currentSpeed = 0;
    private bool attacking = false;
    private int numberFireCharges = 0;
    [SerializeField] private GameObject[] fireCharges;
    private float chargeTimer;

    private void Start()
    {
        updateFireBallObjects();
    }

    public void initializeYlvaLoop()
    {
        StartCoroutine(ylvaLoop());
    }

    IEnumerator ylvaLoop()
    {
        StartCoroutine(chargeFireBalls());
        while (true)
        {
            rotateFirecharges();

            if (attacking == false)
            {
                pickFacingDirection();

                if (Vector2.Distance(transform.position, playerScript.transform.position) > 4)
                {
                    transform.position += (playerScript.transform.position - transform.position).normalized * currentSpeed * Time.deltaTime;
                    if (currentSpeed < 5f)
                    {
                        currentSpeed += Time.deltaTime * 2;
                    }
                    else
                    {
                        currentSpeed = 2.5f;
                    }
                }
                else
                {
                    if (currentSpeed > 0)
                    {
                        transform.position += (playerScript.transform.position - transform.position).normalized * currentSpeed * Time.deltaTime;
                        currentSpeed -= Time.deltaTime * 2;
                        if (currentSpeed < 0)
                        {
                            currentSpeed = 0;
                        }
                    }
                }

            }
            yield return null;
        }
    }

    void pickFacingDirection()
    {
        if(cursorTarget.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-2.5f, 2.5f);
        }
        else
        {
            transform.localScale = new Vector3(2.5f, 2.5f);
        }
    }

    public void triggerFireball(Vector3 target)
    {
        if (numberFireCharges > 0 && attacking == false)
        {
            StartCoroutine(launchFireball(target));
            numberFireCharges--;
            updateFireBallObjects();
        }
    }

    void rotateFirecharges()
    {
        chargeTimer += Time.deltaTime;
        float offSet = 2 * Mathf.PI / numberFireCharges;

        if (chargeTimer > 2 * Mathf.PI)
        {
            chargeTimer = 0;
        }

        for(int i = 0; i < numberFireCharges; i++)
        {
            fireCharges[i].transform.position = transform.position + new Vector3(Mathf.Cos(offSet * i + chargeTimer), Mathf.Sin(offSet * i + chargeTimer)) * 2;
        }
    }

    void updateFireBallObjects()
    {
        for(int i = 0; i < 3; i++)
        {
            if(i < numberFireCharges)
            {
                fireCharges[i].SetActive(true);
            }
            else
            {
                fireCharges[i].SetActive(false);
            }
        }
    }

    IEnumerator chargeFireBalls()
    {
        while (true)
        {
            if(numberFireCharges < 3)
            {
                yield return new WaitForSeconds(8f);
                numberFireCharges++;
                updateFireBallObjects();
            }
            yield return null;
        }
    }

    IEnumerator launchFireball(Vector3 target)
    {
        attacking = true;
        animator.SetTrigger("Attack");
        fireAudio.Play();
        yield return new WaitForSeconds(4/12f);
        GameObject fireBallInstant;
        if(transform.localScale.x < 0)
        {
            fireBallInstant = Instantiate(fireBall, transform.position + Vector3.left, Quaternion.identity);
        }
        else
        {
            fireBallInstant = Instantiate(fireBall, transform.position + Vector3.right, Quaternion.identity);
        }
        fireBallInstant.GetComponent<YlvasFireBall>().angleTravel = Mathf.Atan2(target.y - fireBallInstant.transform.position.y, target.x - fireBallInstant.transform.position.x) * Mathf.Rad2Deg;
        yield return new WaitForSeconds(11/12);
        attacking = false;
    }
}
