using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentEnemy : Enemy {
    GameObject spawnedEmergeAttack;
    public GameObject bubbles;
    public GameObject emergeAttack;
    int numberEmergeAttacks = 0;
    int numberTailAttacks = 0;
    GameObject playerShip;
    public GameObject initBubbles;
    public GameObject tail;
    GameObject spawnedTail;
    public AntiSpawnSpaceDetailer anti;
    public GameObject serpentChest;

    IEnumerator spawnEmergeAttack()
    {
        Vector3 spawnPosition = playerShip.transform.position;
        spawnedEmergeAttack = Instantiate(bubbles, spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.8f);
        Destroy(spawnedEmergeAttack);
        spawnedEmergeAttack = Instantiate(emergeAttack, spawnPosition, Quaternion.identity);
        if(Random.Range(0,2) == 1)
        {
            spawnedEmergeAttack.transform.localScale = new Vector3(-0.8f, 0.8f, 0);
        }
        spawnedEmergeAttack.GetComponent<SeaSerpentEmergeAttack>().seaSerpentEnemy = this;
        yield return new WaitForSeconds(1.75f / 1.5f);
    }

	void Start () {
        playerShip = GameObject.Find("PlayerShip");
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = false;
    }

    void pickEmergeAttack()
    {
        if (spawnedEmergeAttack == null && initBubbles == null)
        {
            numberEmergeAttacks++;
            StartCoroutine(spawnEmergeAttack());
        }
    }

    void pickTailAttack()
    {
        if (spawnedTail == null)
        {
            spawnedTail = Instantiate(tail, transform.position + new Vector3(0, 3.5f, 0), Quaternion.identity);
            spawnedTail.GetComponent<SeaSerpentTail>().seaSerpentEnemy = this;
        }
        else
        {
            SeaSerpentTail seaSerpentTail = spawnedTail.GetComponent<SeaSerpentTail>();
            if (seaSerpentTail.animationCompleted == true)
            {
                if (numberTailAttacks < 4)
                {
                    numberTailAttacks++;
                    if (Random.Range(0, 2) == 1)
                    {
                        seaSerpentTail.smash();
                    }
                    else
                    {
                        seaSerpentTail.swipe();
                    }
                }
                else
                {
                    seaSerpentTail.submerge();
                    spawnedTail.GetComponent<SeaSerpentTail>().animationCompleted = true;
                    numberEmergeAttacks = 0;
                }
            }
        }
    }

	void Update () {
        if (health > 0)
        {
            if (numberEmergeAttacks < 5)
            {
                numberTailAttacks = 0;
                pickEmergeAttack();
            }
            else if(spawnedEmergeAttack == null)
            {
                pickTailAttack();
            }
        }
        else
        {
            if(spawnedTail == null && spawnedEmergeAttack == null)
            {
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
                Instantiate(serpentChest, transform.position, Quaternion.identity);
                anti.trialDefeated = true;
                addKills();
                Destroy(this.gameObject);
            }
            else if(spawnedTail != null && spawnedTail.GetComponent<SeaSerpentTail>().animationCompleted == true)
            {
                spawnedTail.GetComponent<SeaSerpentTail>().submerge();
                spawnedTail.GetComponent<SeaSerpentTail>().animationCompleted = false;
            }
        }
	}
}
