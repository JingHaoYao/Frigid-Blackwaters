using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritHowlBell : ArtifactEffect {
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    GameObject playerShip;
    public GameObject poof, spiritHowl;
    GameObject[] summonedDoggies = new GameObject[3];
    bool summonedDogs = false;
    int numHits = 0;
    public int numDoggiesLeft = 3;
    int prevNumHits = 0;

    IEnumerator spawnDog(Vector3 spawnLocation, int index)
    {
        Instantiate(poof, spawnLocation, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        summonedDoggies[index] = Instantiate(spiritHowl, spawnLocation, Quaternion.identity);
        summonedDoggies[index].transform.parent = this.transform;
        summonedDoggies[index].GetComponent<SpiritHowl>().whatPos = index;
    }

    void summonDoggies()
    {
        FindObjectOfType<AudioManager>().PlaySound("Spirit Howl Summon");
        summonedDogs = true;
        numDoggiesLeft = 3;
        prevNumHits = playerScript.numberHits;
        playerScript.activeEnabled = true;
        numHits = 0;
        Vector3 spawnLocation;
        for (int i = 0; i < 3; i++)
        {
            spawnLocation = playerShip.transform.position + new Vector3(Mathf.Cos(120 * i * Mathf.Deg2Rad), Mathf.Sin(120 * i * Mathf.Deg2Rad), 0) * 1.9f;
            StartCoroutine(spawnDog(spawnLocation, i));
        }
    }

    void Start () {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        playerShip = GameObject.Find("PlayerShip");
    }

	void Update () {
        if (displayItem.isEquipped == true && playerScript.activeEnabled == false && artifacts.numKills >= 6)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonDoggies();
                    artifacts.numKills -= 6;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonDoggies();
                    artifacts.numKills -= 6;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonDoggies();
                    artifacts.numKills -= 6;
                }
            }
        }

        if(summonedDogs == true)
        {
            if (numDoggiesLeft == 0)
            {
                playerScript.activeEnabled = false;
                summonedDogs = false;
            }

            if(this.GetComponent<DisplayItem>().isEquipped == false)
            {
                playerScript.activeEnabled = false;
                summonedDogs = false;
                foreach(GameObject doggy in summonedDoggies)
                {
                    Destroy(doggy);
                }
            }
        }
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        summonedDoggies[numHits].GetComponent<SpiritHowl>().targetAttack = playerScript.damagingObject;
        numHits++;
    }
}
