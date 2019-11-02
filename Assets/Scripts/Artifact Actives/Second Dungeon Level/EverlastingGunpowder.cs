using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EverlastingGunpowder : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    float activePeriod = 0;
    int numberExplosions = 1;
    public GameObject explosion;
    int numberBulletsFired = 0;
    int prevNumberBulletsFired = 0;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 6)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    if (activePeriod <= 0)
                    {
                        artifacts.numKills -= 6;
                        activePeriod = 5;
                        numberExplosions = 1;
                        FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 5);
                        GetComponent<AudioSource>().Play();
                        numberBulletsFired = 0;
                        prevNumberBulletsFired = 0;
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    if (activePeriod <= 0)
                    {
                        artifacts.numKills -= 6;
                        activePeriod = 5;
                        numberExplosions = 1;
                        FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 5);
                        GetComponent<AudioSource>().Play();
                        numberBulletsFired = 0;
                        prevNumberBulletsFired = 0;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    if (activePeriod <= 0)
                    {
                        artifacts.numKills -= 6;
                        activePeriod = 5;
                        numberExplosions = 1;
                        FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 5);
                        GetComponent<AudioSource>().Play();
                        numberBulletsFired = 0;
                        prevNumberBulletsFired = 0;
                    }
                }
            }
        }

        if(activePeriod > 0)
        {
            activePeriod -= Time.deltaTime;
        }
    }

    public override void addedKill(string tag, Vector3 deathPos)
    {
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
    }
    // Whenever the player fires the left weapon, and so on
    public override void firedLeftWeapon(GameObject[] bullet)
    {
        numberBulletsFired++;
    }
    public override void firedFrontWeapon(GameObject[] bullet)
    {
        numberBulletsFired++;
    }
    public override void firedRightWeapon(GameObject[] bullet)
    {
        numberBulletsFired++;
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

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        if(activePeriod > 0 && numberBulletsFired != prevNumberBulletsFired)
        {
            StartCoroutine(spawnExplosions(numberExplosions, enemy));
            numberExplosions++;
            prevNumberBulletsFired = numberBulletsFired;
        }
    }

    IEnumerator spawnExplosions(int numberExplosions, Enemy enemy)
    {
        for(int i = 0; i < numberExplosions; i++)
        {
            Instantiate(explosion, enemy.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            yield return new WaitForSeconds(0.2f);
        }
    }
}
