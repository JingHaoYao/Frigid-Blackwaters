using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSeeingAmulet : ArtifactBonus
{
    [SerializeField] GameObject allSeeingStatusEffect;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] AudioSource audioSource;
    bool eyesActive = false;

    IEnumerator waitForDuration()
    {
        eyesActive = true;
        yield return new WaitForSeconds(4.5f);
        eyesActive = false;
    }

    void summonEyes()
    {
        if (eyesActive == false && !EnemyPool.isPoolEmpty())
        {
            audioSource.Play();
            PlayerProperties.playerArtifacts.numKills -= killRequirement;
            foreach (Enemy enemy in EnemyPool.enemyPool)
            {
                GameObject effectInstant = Instantiate(allSeeingStatusEffect, enemy.transform.position, Quaternion.identity);
                enemy.addStatus(effectInstant.GetComponent<EnemyStatusEffect>(), 4);
            }

            StartCoroutine(waitForDuration());
        }
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonEyes();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonEyes();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonEyes();
                }
            }
        }
    }
}
