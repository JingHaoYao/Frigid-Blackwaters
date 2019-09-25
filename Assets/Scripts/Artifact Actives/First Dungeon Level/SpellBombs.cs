using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBombs : MonoBehaviour {
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    GameObject playerShip, spawnedSpellBomb;
    public GameObject spellBomb;
    bool spawnedBomb = false;
    public int numSpellBombs = 0;

    void spawnBomb()
    {
        spawnedSpellBomb = Instantiate(spellBomb, playerShip.transform.position, Quaternion.identity);
        spawnedSpellBomb.transform.parent = this.transform;
        numSpellBombs++;
        playerScript.activeEnabled = true;
        spawnedBomb = true;
    }

    void Start () {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        playerShip = GameObject.Find("PlayerShip");
    }

	void Update () {
        if (displayItem.isEquipped == true && numSpellBombs < 3 && artifacts.numKills >= 1)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    artifacts.numKills -= 1;
                    spawnBomb();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    artifacts.numKills -= 1;
                    spawnBomb();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    artifacts.numKills -= 1;
                    spawnBomb();
                }
            }
        }

        if(spawnedBomb == true && spawnedSpellBomb == null)
        {
            playerScript.activeEnabled = false;
            spawnedBomb = false;
        }
    }
}
