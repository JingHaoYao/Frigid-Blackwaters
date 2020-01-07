using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrustaceaCrystalShard : ArtifactBonus
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject crystal;
    bool placingCrystal;

    public List<CrustaceaCrystalShardCrystalObstacle> crystals = new List<CrustaceaCrystalShardCrystalObstacle>();

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    if (crystals.Count < 3)
                    {
                        artifacts.numKills -= killRequirement;
                        placingCrystal = true;
                        GetComponent<AudioSource>().Play();
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    if (crystals.Count < 3)
                    {
                        artifacts.numKills -= killRequirement;
                        placingCrystal = true;
                        GetComponent<AudioSource>().Play();
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    if (crystals.Count < 3)
                    {
                        artifacts.numKills -= killRequirement;
                        placingCrystal = true;
                        GetComponent<AudioSource>().Play();
                    }
                }
            }
        }

        if (displayItem.isEquipped == false && placingCrystal == true)
        {
            placingCrystal = false;
        }

        if (placingCrystal == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject crystalInstant = Instantiate(crystal, new Vector3(
                    Mathf.Clamp(FindObjectOfType<CursorTarget>().transform.position.x, Camera.main.transform.position.x - 8, Camera.main.transform.position.x + 8),
                    Mathf.Clamp(FindObjectOfType<CursorTarget>().transform.position.y, Camera.main.transform.position.y - 8, Camera.main.transform.position.y + 8)),
                    Quaternion.identity);
                crystalInstant.GetComponent<CrustaceaCrystalShardCrystalObstacle>().shard = this;
                crystals.Add(crystalInstant.GetComponent<CrustaceaCrystalShardCrystalObstacle>());
                placingCrystal = false;
            }
        }
    }
}
