using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : MonoBehaviour {
    GameObject playerShip;
    PlayerScript playerScript;
    public Animator animator;
    string triggerName;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;

        if ((playerScript.angleEffect >= 67.5f && playerScript.angleEffect < 112.5f) || (playerScript.angleEffect >= 247.5f && playerScript.angleEffect < 292.5f))
        {
            if (triggerName != "Lightning3")
            {
                triggerName = "Lightning3";
                animator.SetTrigger(triggerName);
                transform.localScale = new Vector3(0.35f, 0.35f, 0);
            }
        }
        else if ((playerScript.angleEffect >= 22.5f && playerScript.angleEffect < 67.5f) || (playerScript.angleEffect > 202.5f && playerScript.angleEffect <= 247.5f))
        {
            if(triggerName != "Lightning2"){
                triggerName = "Lightning2";
                animator.SetTrigger(triggerName);
                transform.localScale = new Vector3(-0.35f, 0.35f, 0);
            }
        }
        else if ((playerScript.angleEffect >= 112.5f && playerScript.angleEffect < 157.5f) || (playerScript.angleEffect >= 292.5f && playerScript.angleEffect < 337.5f))
        {
            if (triggerName != "Lightning2")
            {
                triggerName = "Lightning2";
                animator.SetTrigger(triggerName);
                transform.localScale = new Vector3(0.35f, 0.35f, 0);
            }
        }
        else
        {
            if (triggerName != "Lightning1")
            {
                triggerName = "Lightning1";
                animator.SetTrigger(triggerName);
                transform.localScale = new Vector3(0.35f, 0.35f, 0);
            }
        }

        if(playerScript.angleEffect >= 157.5f && playerScript.angleEffect < 202.5f)
        {
            transform.position = playerShip.transform.position + new Vector3(-0.4f, 0, 0);
        }
        else if(playerScript.angleEffect >= 22.5f && playerScript.angleEffect < 67.5f)
        {
            transform.position = playerShip.transform.position + new Vector3(0.2f, 0.3f, 0);
        }
        else if (playerScript.angleEffect < 157.5 && playerScript.angleEffect >= 112.5f)
        {
            transform.position = playerShip.transform.position + new Vector3(-0.2f, 0.3f, 0);
        }
        else
        {
            transform.position = playerShip.transform.position;
        }
    }
}
