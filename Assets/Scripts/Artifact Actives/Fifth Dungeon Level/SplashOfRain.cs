using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashOfRain : ArtifactEffect
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] AudioSource audioSource;
    bool isSummoning = false;

    [SerializeField] GameObject rainDrop;

    float angleToCursor()
    {
        return Mathf.Atan2(PlayerProperties.cursorPosition.y - PlayerProperties.playerShipPosition.y, PlayerProperties.cursorPosition.x - PlayerProperties.playerShipPosition.x);
    }

    IEnumerator summonTearDrops()
    {
        audioSource.Play();
        isSummoning = true;
        for(int i = 0; i < 20; i++)
        {
            GameObject rainDropInstant = Instantiate(rainDrop, PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleToCursor()), Mathf.Sin(angleToCursor())) * 1.5f, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        isSummoning = false;
    }
     
    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    if (isSummoning == false)
                    {
                        PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                        StartCoroutine(summonTearDrops());
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    if (isSummoning == false)
                    {
                        PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                        StartCoroutine(summonTearDrops());
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    if (isSummoning == false)
                    {
                        PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                        StartCoroutine(summonTearDrops());
                    }
                }
            }
        }
    }
}
