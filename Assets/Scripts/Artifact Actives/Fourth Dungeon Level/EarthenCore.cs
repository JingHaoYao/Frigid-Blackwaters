using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthenCore : ArtifactEffect
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    List<GameObject> spawnedOrbs = new List<GameObject>();
    List<int> indexList = new List<int>();
    private int orbCount = 0;
    [SerializeField] GameObject earthenCore;
    [SerializeField] AudioSource activateAudio;
    float rotatePeriod = 0;

    public void removeOrb(GameObject gameObject)
    {
        spawnedOrbs.Remove(gameObject);
        indexList.Remove(indexList[0]);
        orbCount--;
    }

    private void Start()
    {
        StartCoroutine(mainRotateLoop());
    }

    void addOrb()
    {
        if (orbCount < 5)
        {
            activateAudio.Play();
            PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
            GameObject instant = Instantiate(earthenCore, PlayerProperties.playerShipPosition, Quaternion.identity);
            instant.GetComponent<EarthenCoreOrb>().earthenCore = this;
            spawnedOrbs.Add(instant);

            for (int i = 0; i < 5; i++)
            {
                if (!indexList.Contains(i))
                {
                    indexList.Add(i);
                }
            }

            orbCount++;
        }
    }

    IEnumerator mainRotateLoop()
    {
        while (true)
        {
            if (orbCount > 0)
            {
                rotatePeriod += Time.deltaTime * 2;
                if (rotatePeriod > Mathf.PI * 2)
                {
                    rotatePeriod = 0;
                }

                for (int i = 0; i < orbCount; i++)
                {
                    float angleOffset = (360 / 5) * indexList[i] * Mathf.Deg2Rad;
                    spawnedOrbs[i].transform.position = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleOffset + rotatePeriod), Mathf.Sin(angleOffset + rotatePeriod)) * 3;
                }
            }

            yield return null;
        }
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    addOrb();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    addOrb();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    addOrb();
                }
            }
        }
    }


}
