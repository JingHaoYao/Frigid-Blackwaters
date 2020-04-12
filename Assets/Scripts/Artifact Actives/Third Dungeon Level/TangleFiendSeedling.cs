using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangleFiendSeedling : MonoBehaviour
{
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] LayerMask layerFilter;
    Camera mainCamera;
    public GameObject vineSpike1, vineSpike2;
    bool spawningSpikes = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    bool checkIfPositionIsValid(Vector3 pos)
    {
        return !Physics2D.OverlapCircle(pos, 0.5f, layerFilter);
    }

    IEnumerator spawnSpikes()
    {
        spawningSpikes = true;
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 3);
        Vector3 positionToSpawn = PlayerProperties.playerShipPosition;
        for(int i = 0; i < 10; i++)
        {
            Vector3 previousSpawnPosition = positionToSpawn;
            float angle = Mathf.Atan2(PlayerProperties.cursorPosition.y - previousSpawnPosition.y, PlayerProperties.cursorPosition.x - previousSpawnPosition.x);
            positionToSpawn = previousSpawnPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            if (checkIfPositionIsValid(positionToSpawn))
            {
                if (Random.Range(0, 2) == 1)
                {
                    GameObject spike = Instantiate(vineSpike1, positionToSpawn, Quaternion.identity);
                    spike.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                    if (Random.Range(0, 2) == 1)
                    {
                        Vector3 scale = spike.transform.localScale;
                        spike.transform.localScale = new Vector3(scale.x * -1, scale.y);
                    }
                }
                else
                {
                    GameObject spike = Instantiate(vineSpike2, positionToSpawn, Quaternion.identity);
                    spike.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                    if (Random.Range(0, 2) == 1)
                    {
                        Vector3 scale = spike.transform.localScale;
                        spike.transform.localScale = new Vector3(scale.x * -1, scale.y);
                    }
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
        spawningSpikes = false;
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement && spawningSpikes == false)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    StartCoroutine(spawnSpikes());
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    StartCoroutine(spawnSpikes());
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    StartCoroutine(spawnSpikes());
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
        }
    }
}
