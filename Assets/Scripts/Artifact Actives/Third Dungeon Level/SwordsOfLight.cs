using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsOfLight : MonoBehaviour
{
    [SerializeField] DisplayItem displayItem;
    PlayerScript playerScript;
    public GameObject swordOfLight;
    [SerializeField] AudioSource activatedAudio;
    [SerializeField] ArtifactBonus artifactBonus;

    void Start()
    {
        playerScript = PlayerProperties.playerScript;
    }

    void spawnSwordsOfLight()
    {
        activatedAudio.Play();
        PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
        for(int i = 0; i < 4; i++)
        {
            float angle = i * 90 * Mathf.Deg2Rad;
            Instantiate(swordOfLight, PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 3, Quaternion.identity);
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    spawnSwordsOfLight();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    spawnSwordsOfLight();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    spawnSwordsOfLight();
                }
            }
        }
    }
}
