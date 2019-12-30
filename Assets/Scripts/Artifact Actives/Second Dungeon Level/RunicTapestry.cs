using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunicTapestry : ArtifactBonus
{
    public GameObject depthBomb;
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void activateArtifact()
    {
        artifacts.numKills -= killRequirement;
        StartCoroutine(summonDepthBombs(Mathf.Atan2((FindObjectOfType<CursorTarget>().transform.position - playerScript.transform.position).y, (FindObjectOfType<CursorTarget>().transform.position - playerScript.transform.position).x) * Mathf.Rad2Deg));
    }

    IEnumerator summonDepthBombs(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 spawnPosition = playerScript.transform.position;
        while (Mathf.Abs(Camera.main.transform.position.x - spawnPosition.x) < 7.5f && Mathf.Abs(Camera.main.transform.position.y - spawnPosition.y) < 7.5f)
        {
            spawnPosition += directionVector * 2.5f;
        }
        spawnPosition += directionVector * -2.5f;
        while (Mathf.Abs(Camera.main.transform.position.x - spawnPosition.x) < 8f && Mathf.Abs(Camera.main.transform.position.y - spawnPosition.y) < 8f)
        {
            GameObject instant = Instantiate(depthBomb, spawnPosition, Quaternion.identity);
            spawnPosition += directionVector * -2.5f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    activateArtifact();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    activateArtifact();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    activateArtifact();
                }
            }
        }
    }
}
