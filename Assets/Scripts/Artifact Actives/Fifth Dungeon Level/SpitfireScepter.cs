using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitfireScepter : MonoBehaviour
{
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] AudioSource audioSource;

    [SerializeField] GameObject spitFireProjectile, spitFireParticles;

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement && EnemyPool.enemyPool.Count > 0)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    FireProjectiles();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    FireProjectiles();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    FireProjectiles();
                }
            }
        }
    }

    void FireProjectiles()
    {
        audioSource.Play();
        int enemyCount = EnemyPool.enemyPool.Count;
        float offset = 360 / enemyCount;

        for(int i = 0; i < enemyCount; i++)
        {
            float angleInRad = offset * i * Mathf.Deg2Rad;
            Instantiate(spitFireParticles, PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad)), Quaternion.identity);

            GameObject projectileInstant = Instantiate(spitFireProjectile, PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad)), Quaternion.identity);
            projectileInstant.GetComponent<SpitfireProjectile>().Initialize(EnemyPool.enemyPool[i]);
        }
    }
}
