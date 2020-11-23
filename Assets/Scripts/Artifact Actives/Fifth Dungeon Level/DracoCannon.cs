using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DracoCannon : ArtifactEffect
{
    [SerializeField] GameObject dragonCannon;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    DragonCannon dragonCannonInstant;
    bool onCooldown = false;

    public override void firedFrontWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        if(dragonCannonInstant != null)
        {
            dragonCannonInstant.ShootProjectile();
        }
    }

    public override void firedLeftWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        if (dragonCannonInstant != null)
        {
            dragonCannonInstant.ShootProjectile();
        }
    }

    public override void firedRightWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        if (dragonCannonInstant != null)
        {
            dragonCannonInstant.ShootProjectile();
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement && onCooldown == false)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    SummonDragonCannon(PlayerProperties.playerShipPosition);
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    SummonDragonCannon(PlayerProperties.playerShipPosition);
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    SummonDragonCannon(PlayerProperties.playerShipPosition);
                }
            }
        }
    }

    public override void artifactUnequipped()
    {
        if (dragonCannonInstant != null)
        {
            Destroy(dragonCannonInstant.gameObject);
        }
    }

    IEnumerator cooldownPeriod()
    {
        onCooldown = true;
        yield return new WaitForSeconds(5f);
        onCooldown = false;
    }

    void SummonDragonCannon(Vector3 position)
    {
        if(dragonCannonInstant == null)
        {
            dragonCannonInstant = Instantiate(dragonCannon, PlayerProperties.playerShipPosition, Quaternion.identity).GetComponent<DragonCannon>();
        }
        dragonCannonInstant.relocateCannon(position);
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 5f);
        StartCoroutine(cooldownPeriod());
    }
}
