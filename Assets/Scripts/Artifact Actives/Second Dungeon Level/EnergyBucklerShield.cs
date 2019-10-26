using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBucklerShield : MonoBehaviour
{
    Animator animator;
    PlayerScript playerScript;
    public float respawnPeriod = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerScript = FindObjectOfType<PlayerScript>();
    }

    void Update()
    {

        transform.position = playerScript.transform.position + new Vector3(0, -1.2f, 0);

        if (respawnPeriod <= 0)
        {
            playerScript.damageImmunity = true;
        }
        else
        {
            if (playerScript.damageImmunity == true)
            {
                playerScript.damageImmunity = false;
                animator.SetTrigger("Break");
            }
            respawnPeriod -= Time.deltaTime;

            if(respawnPeriod <= 0)
            {
                respawnPeriod = 0;
                animator.SetTrigger("Reform");
            }
        }
    }
}
