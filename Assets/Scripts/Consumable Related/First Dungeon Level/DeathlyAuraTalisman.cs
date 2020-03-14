using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathlyAuraTalisman : MonoBehaviour {
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    public GameObject deathlyAuraBall;

    void spawnBall()
    {
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 30);
        PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
        Instantiate(deathlyAuraBall, GameObject.Find("PlayerShip").transform.position + new Vector3(0, 2, 0), Quaternion.identity);
    }

	void Update () {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    spawnBall();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    spawnBall();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    spawnBall();
                }
            }
        }
    }
}
