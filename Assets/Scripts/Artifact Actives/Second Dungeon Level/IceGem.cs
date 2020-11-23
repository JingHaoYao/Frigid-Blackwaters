using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGem : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject iceMissile;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void summonIcicles(float angleAttack)
    {
        GetComponent<AudioSource>().Play();
        for(int i = 0; i < 5; i++)
        {
            float angle = angleAttack - 20 + 10 * i;
            GameObject iceMissileInstant = Instantiate(iceMissile, playerScript.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 2f, Quaternion.Euler(0, 0, angle));
            iceMissileInstant.GetComponent<IceGemMissile>().angleTravel = angle;
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 7)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    artifacts.numKills -= 7;
                    summonIcicles(playerScript.whatAngleTraveled);
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    artifacts.numKills -= 7;
                    summonIcicles(playerScript.whatAngleTraveled);
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    artifacts.numKills -= 7;
                    summonIcicles(playerScript.whatAngleTraveled);
                }
            }
        }
    }
}
