using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornofEverlastingWind : MonoBehaviour {
    DisplayItem displayItem;
    PlayerScript playerScript;
    Artifacts artifacts;
    public GameObject windEffect;
    public GameObject windBurst;
    bool windActivated = false;

    IEnumerator speedBoost()
    {
        windActivated = true;
        GameObject windEff = Instantiate(windEffect, GameObject.Find("PlayerShip").transform.position, Quaternion.Euler(0,0,playerScript.angleEffect));
        Instantiate(windBurst, GameObject.Find("PlayerShip").transform.position, Quaternion.Euler(0, 0, playerScript.angleEffect + 90));
        artifacts.numKills -= 2;
        float tempSpeed = playerScript.boatSpeed;
        playerScript.boatSpeed += 3;
        FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, 3);
        yield return new WaitForSeconds(3);
        windEff.GetComponent<WindEffect>().animator.SetTrigger("FadeOut");
        Destroy(windEff, 0.333f);
        playerScript.boatSpeed = tempSpeed;
        yield return new WaitForSeconds(0.333f);
        windActivated = false;
    }

	void Start () {
        displayItem = GetComponent<DisplayItem>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
	}

	void Update () {
		if(displayItem.isEquipped == true && windActivated == false && artifacts.numKills >= 2)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    StartCoroutine(speedBoost());
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    StartCoroutine(speedBoost());
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    StartCoroutine(speedBoost());
                }
            }
        }
	}
}
