using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniDemonHorn : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    bool activated = false;
    float damagePeriod = 0;
    public GameObject oniAura;
    GameObject oniAuraInstant;
    float coolDownPeriod = 0;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
        oniAuraInstant = Instantiate(oniAura, playerScript.transform.position, Quaternion.identity);
        oniAura.GetComponent<FollowObject>().objectToFollow = playerScript.gameObject;
        oniAuraInstant.SetActive(false);
    }

    IEnumerator turnOffAura()
    {
        oniAuraInstant.GetComponent<Animator>().SetTrigger("Dissipate");
        yield return new WaitForSeconds(0.333f);
        oniAuraInstant.SetActive(false);
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && coolDownPeriod <= 0)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    if(activated == false)
                    {
                        activated = true;
                        GetComponents<AudioSource>()[0].Play();
                        artifactBonus.attackBonus = 3;
                        artifactBonus.speedBonus = 2;
                        oniAuraInstant.SetActive(true);
                        oniAuraInstant.GetComponent<Animator>().SetTrigger("Form");
                        artifacts.UpdateUI();
                    }
                    else
                    {
                        StartCoroutine(turnOffAura());
                        activated = false;
                        artifactBonus.attackBonus = 0;
                        artifactBonus.speedBonus = 0;
                        artifacts.UpdateUI();
                        coolDownPeriod = 2;
                        FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 2);
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {

                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {

                }
            }
        }

        if(coolDownPeriod > 0)
        {
            coolDownPeriod -= Time.deltaTime;
        }

        if (activated)
        {
            if(damagePeriod < 1)
            {
                damagePeriod += Time.deltaTime;
            }
            else
            {
                damagePeriod = 0;
                playerScript.amountDamage += 50;
                GetComponents<AudioSource>()[1].Play();
            }
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
