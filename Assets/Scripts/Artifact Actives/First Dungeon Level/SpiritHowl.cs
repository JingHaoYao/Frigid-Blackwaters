using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritHowl : MonoBehaviour {
    GameObject playerShip;
    Vector3 travelDirection;
    Animator animator;
    public int whatHowl = 0;
    public GameObject targetAttack;
    public GameObject poof, chomp;
    GameObject trueTarget;
    bool setTarget = false;
    float rotationAngle = 0;
    public float whatPos = 0;

    void Start () {
        playerShip = GameObject.Find("PlayerShip");
        animator = GetComponent<Animator>();
        rotationAngle = whatPos * ((2 * Mathf.PI) / 3);
    }

    void moveTowards(float direction)
    {
        travelDirection = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0);
    }

    void pickSprite(float direction)
    {
        if (direction > 75 && direction < 105)
        {
            animator.SetTrigger("View3");
            transform.localScale = new Vector3(0.1f, 0.1f, 0);
        }
        else if (direction < 285 && direction > 265)
        {
            animator.SetTrigger("View2");
            transform.localScale = new Vector3(0.1f, 0.1f, 0);
        }
        else if (direction >= 285 && direction <= 360)
        {
            animator.SetTrigger("View1");
            transform.localScale = new Vector3(0.1f, 0.1f, 0);
        }
        else if (direction >= 180 && direction <= 265)
        {
            animator.SetTrigger("View1");
            transform.localScale = new Vector3(-0.1f, 0.1f, 0);
        }
        else if (direction < 180 && direction >= 105)
        {
            animator.SetTrigger("View4");
            transform.localScale = new Vector3(-0.1f, 0.1f, 0);
        }
        else
        {
            animator.SetTrigger("View4");
            transform.localScale = new Vector3(0.1f, 0.1f, 0);
        }
    }

    void Update () {

        if (targetAttack == null && setTarget == false)
        {
            /*float circleAngle = (270 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            pickSprite(circleAngle);
            float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            travelDirection = new Vector3(Mathf.Cos(circleAngle * Mathf.Deg2Rad), Mathf.Sin(circleAngle * Mathf.Deg2Rad)) * 3;
            if (Vector2.Distance(transform.position, playerShip.transform.position) < 1.8)
            {
                travelDirection += new Vector3(Mathf.Cos((angleToShip + 180) * Mathf.Deg2Rad), Mathf.Sin((angleToShip + 180) * Mathf.Deg2Rad), 0) * (playerShip.GetComponent<PlayerScript>().boatSpeed + playerShip.GetComponent<PlayerScript>().speedBonus);
            }
            else if (Vector2.Distance(transform.position, playerShip.transform.position) > 2f)
            {
                travelDirection += new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad), 0) * (playerShip.GetComponent<PlayerScript>().boatSpeed + playerShip.GetComponent<PlayerScript>().speedBonus);
            }*/
            rotationAngle += Time.deltaTime;
            float directionAngle = (360 + (rotationAngle + Mathf.PI/2)* Mathf.Rad2Deg) % 360;
            pickSprite(directionAngle);
            if(rotationAngle >= Mathf.PI * 2)
            {
                rotationAngle = 0;
            }
            transform.position = playerShip.transform.position + new Vector3(Mathf.Cos(rotationAngle), Mathf.Sin(rotationAngle), 0) * 2;
        }
        else
        {
            if (targetAttack != null && setTarget == false)
            {
                if(targetAttack.GetComponent<ProjectileParent>())
                {
                    trueTarget = targetAttack.GetComponent<ProjectileParent>().instantiater;
                }
                else if (targetAttack.transform.parent != null)
                {
                    trueTarget = targetAttack.transform.parent.gameObject;
                }
                else
                {
                    trueTarget = targetAttack;
                }
                setTarget = true;
            }

            if(setTarget == true && targetAttack == null)
            {
                Instantiate(poof, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }

            float angleToTarget = Mathf.Atan2(trueTarget.transform.position.y - transform.position.y, trueTarget.transform.position.x - transform.position.x);
            travelDirection = new Vector3(Mathf.Cos(angleToTarget), Mathf.Sin(angleToTarget), 0) * 8;
            pickSprite((360 + angleToTarget * Mathf.Rad2Deg) % 360);
            transform.position += travelDirection * Time.deltaTime;
            if (Vector2.Distance(trueTarget.transform.position, transform.position) < 0.3f)
            {
                Instantiate(poof, transform.position, Quaternion.identity);
                Instantiate(chomp, trueTarget.transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
                transform.parent.gameObject.GetComponent<SpiritHowlBell>().numDoggiesLeft--;
                Destroy(this.gameObject);
            }
        }
    }
}
