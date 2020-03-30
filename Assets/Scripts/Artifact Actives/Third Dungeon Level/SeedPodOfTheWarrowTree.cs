using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPodOfTheWarrowTree : MonoBehaviour
{
    public GameObject[] vines;
    List<GameObject> spawnedVines = new List<GameObject>();
    Camera mainCamera;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    StartCoroutine(spawnVines());
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    StartCoroutine(spawnVines()); ;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    StartCoroutine(spawnVines());
                }
            }
        }
    }

    IEnumerator spawnVines()
    {
        PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
        Vector3 originalShipPosition = PlayerProperties.playerShipPosition;
        Vector3 cameraPosition = mainCamera.transform.position;
        bool continueIterating = true;
        int index = 0;
        while (continueIterating)
        {
            continueIterating = false;
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45 * Mathf.Deg2Rad;
                Vector3 positionToConsider = originalShipPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * (3 * index + 1.5f);
                if(Mathf.Abs(cameraPosition.x - positionToConsider.x) < 8.5f && Mathf.Abs(cameraPosition.y - positionToConsider.y) < 8.5f)
                {
                    continueIterating = true;
                    pickVineToSpawn(angle, positionToConsider);
                }
            }
            index++;
            yield return new WaitForSeconds(8 / 12f);
        }
        yield return new WaitForSeconds(2);
    }

    void pickVineToSpawn(float angleInRad, Vector3 positionToSpawn)
    {
        float angleOrientation = (angleInRad * Mathf.Rad2Deg + 360) % 360;

        GameObject vineInstant;
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            vineInstant = Instantiate(vines[1], positionToSpawn, Quaternion.identity);
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            vineInstant = Instantiate(vines[4], positionToSpawn, Quaternion.identity);
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            vineInstant = Instantiate(vines[1], positionToSpawn, Quaternion.identity);
            Vector3 currScale = vineInstant.transform.localScale;
            vineInstant.transform.localScale = new Vector3(currScale.x * -1, currScale.y);
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            vineInstant = Instantiate(vines[0], positionToSpawn, Quaternion.identity);
            Vector3 currScale = vineInstant.transform.localScale;
            vineInstant.transform.localScale = new Vector3(currScale.x * -1, currScale.y);
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            vineInstant = Instantiate(vines[2], positionToSpawn, Quaternion.identity);
            Vector3 currScale = vineInstant.transform.localScale;
            vineInstant.transform.localScale = new Vector3(currScale.x * -1, currScale.y);
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            vineInstant = Instantiate(vines[3], positionToSpawn, Quaternion.identity);
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            vineInstant = Instantiate(vines[2], positionToSpawn, Quaternion.identity);
        }
        else
        {
            vineInstant = Instantiate(vines[0], positionToSpawn, Quaternion.identity);
        }
        spawnedVines.Add(vineInstant);
    }
}
