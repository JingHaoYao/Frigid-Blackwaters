using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderGodsBow : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    public GameObject thunderGodsZap;
    bool firingZap;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 5)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    artifacts.numKills -= 5;
                    firingZap = true;
                    GetComponent<AudioSource>().Play();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    artifacts.numKills -= 5;
                    firingZap = true;
                    GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    artifacts.numKills -= 5;
                    firingZap = true;
                    GetComponent<AudioSource>().Play();
                }
            }
        }

        if (displayItem.isEquipped == false && firingZap == true)
        {
            firingZap = false;
        }

        if (firingZap == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(thunderGodsZap, new Vector3(
                    Mathf.Clamp(FindObjectOfType<CursorTarget>().transform.position.x, Camera.main.transform.position.x - 9, Camera.main.transform.position.x + 9), 
                    Mathf.Clamp(FindObjectOfType<CursorTarget>().transform.position.y, Camera.main.transform.position.y - 9, Camera.main.transform.position.y + 9)), 
                    Quaternion.identity);
                firingZap = false;
            }
        }
    }
}

