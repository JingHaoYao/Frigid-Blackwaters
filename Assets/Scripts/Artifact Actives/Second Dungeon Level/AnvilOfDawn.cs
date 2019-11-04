using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilOfDawn : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    public GameObject lightningStrike;
    bool zapping = false;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    bool checkPos(Vector3 pos, List<Vector3> list)
    {
        foreach (Vector3 positions in list)
        {
            if (Vector2.Distance(positions, pos) < 1)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator spawnZaps()
    {
        zapping = true;
        List<Vector3> alreadySpawnedPositions = new List<Vector3>();
        for(int i = 0; i < 15; i++)
        {
            Vector3 randPos = new Vector3(
                Mathf.Clamp(playerScript.transform.position.x + Random.Range(-6.0f, 6.0f), Camera.main.transform.position.x - 9, Camera.main.transform.position.x + 9),
                 Mathf.Clamp(playerScript.transform.position.y + Random.Range(-6.0f, 6.0f), Camera.main.transform.position.y - 9, Camera.main.transform.position.y + 9));

            while (checkPos(randPos, alreadySpawnedPositions) == false)
            {
                randPos = new Vector3(
                        Mathf.Clamp(playerScript.transform.position.x + Random.Range(-6.0f, 6.0f), Camera.main.transform.position.x - 9, Camera.main.transform.position.x + 9),
                        Mathf.Clamp(playerScript.transform.position.y + Random.Range(-6.0f, 6.0f), Camera.main.transform.position.y - 9, Camera.main.transform.position.y + 9));
            }

            GameObject strike = Instantiate(lightningStrike, randPos, Quaternion.identity);
            Vector3 scale = strike.transform.localScale;
            if(Random.Range(0,2) == 1)
            {
                strike.transform.localScale = new Vector3(-scale.x, scale.y);
            }
            yield return new WaitForSeconds(0.2f);
        }
        zapping = false;
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 12)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    if (zapping == false)
                    {
                        artifacts.numKills -= 12;
                        StartCoroutine(spawnZaps());
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    if (zapping == false)
                    {
                        artifacts.numKills -= 12;
                        StartCoroutine(spawnZaps());
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    if (zapping == false)
                    {
                        artifacts.numKills -= 12;
                        StartCoroutine(spawnZaps());
                    }
                }
            }
        }
    }
}
