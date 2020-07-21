using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBuckler : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject energyShield;
    GameObject energyShieldInstant;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (displayItem.isEquipped == false)
        {
            if(energyShieldInstant != null)
            {
                Destroy(energyShieldInstant);
            }
        }
        else
        {
            if(energyShieldInstant == null)
            {
                energyShieldInstant = Instantiate(energyShield, playerScript.transform.position + new Vector3(0, -1.3f, 0), Quaternion.identity);
            }
        }
    }

    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if (energyShieldInstant.GetComponent<EnergyBucklerShield>().respawnPeriod <= 0)
        {
            energyShieldInstant.GetComponent<EnergyBucklerShield>().breakShield(20);
            FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, 20);
        }   
    }
}
