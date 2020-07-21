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

    public void breakShield(float duration)
    {
        respawnPeriod = duration;
        animator.SetTrigger("Break");
        playerScript.removeImmunityItem(this.gameObject);
    }

    void Update()
    {

        transform.position = playerScript.transform.position + new Vector3(0, -1.2f, 0);

        if (respawnPeriod <= 0)
        {
            playerScript.addImmunityItem(this.gameObject);
        }
        else
        {
            respawnPeriod -= Time.deltaTime;

            if(respawnPeriod <= 0)
            {
                respawnPeriod = 0;
                animator.SetTrigger("Reform");
            }
        }
    }

    private void OnDestroy()
    {
        playerScript.removeImmunityItem(this.gameObject);
    }
}
