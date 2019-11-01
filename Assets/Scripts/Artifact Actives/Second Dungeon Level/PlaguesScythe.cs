using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaguesScythe : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    float artifactTimer = 0;
    public GameObject plagueParticles;
    public GameObject activatedEffect;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 4)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    if (artifactTimer <= 0)
                    {
                        artifactTimer = 5;
                        artifacts.numKills -= 4;
                        this.GetComponent<AudioSource>().Play();
                        FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, 5);
                        GameObject effect = Instantiate(activatedEffect, playerScript.transform.position, Quaternion.identity);
                        effect.GetComponent<FollowObject>().objectToFollow = playerScript.gameObject;
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    if (artifactTimer <= 0)
                    {
                        artifactTimer = 5;
                        artifacts.numKills -= 4;
                        this.GetComponent<AudioSource>().Play();
                        FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, 5);
                        GameObject effect = Instantiate(activatedEffect, playerScript.transform.position, Quaternion.identity);
                        effect.GetComponent<FollowObject>().objectToFollow = playerScript.gameObject;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    if (artifactTimer <= 0)
                    {
                        artifactTimer = 5;
                        artifacts.numKills -= 4;
                        this.GetComponent<AudioSource>().Play();
                        FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, 5);
                        GameObject effect = Instantiate(activatedEffect, playerScript.transform.position, Quaternion.identity);
                        effect.GetComponent<FollowObject>().objectToFollow = playerScript.gameObject;
                    }
                }
            }
        }

        if(artifactTimer > 0)
        {
            artifactTimer -= Time.deltaTime;
        }
    }

    public override void addedKill(string tag, Vector3 deathPos)
    {
        if(artifactTimer > 0)
        {
            Instantiate(plagueParticles, deathPos + new Vector3(0, 0.7f, 0), Quaternion.identity);
            artifactBonus.healthBonus += 100;
            artifactBonus.speedBonus += 0.1f;
        }
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
    }
    // Whenever the player fires the left weapon, and so on
    public override void firedLeftWeapon(GameObject[] bullet)
    {
    }
    public override void firedFrontWeapon(GameObject[] bullet)
    {
    }
    public override void firedRightWeapon(GameObject[] bullet)
    {
    }
    // Whenever the player enters a previously unentered room
    public override void exploredNewRoom(int whatRoomType) { }
    // Whenever the player picks up an item (updates the inventory)
    public override void updatedInventory()
    {
    }
    // whenever the player dashes
    public override void playerDashed()
    {
    }

    public override void dealtDamage(int damageDealt)
    {
    }
}
