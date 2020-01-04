using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeGuardiansMace : ArtifactBonus
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    GameObject maceInstant;
    public GameObject mace;
    float macePeriod = 0;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true)
        {
            if(maceInstant == null)
            {
                macePeriod = Random.Range(0f, 2 * Mathf.PI);
                maceInstant = Instantiate(mace, playerScript.transform.position + new Vector3(Mathf.Cos(macePeriod), Mathf.Sin(macePeriod)), Quaternion.identity);
            }

            macePeriod += Time.deltaTime * 2;
            if(macePeriod > Mathf.PI * 2)
            {
                macePeriod = 0;
            }
            maceInstant.transform.position = playerScript.transform.position + new Vector3(Mathf.Cos(macePeriod), Mathf.Sin(macePeriod)) * 3;
        }
        else
        {
            if(maceInstant != null)
            {
                maceInstant.GetComponent<TomeGuardiansMaceInstant>().unequipArtifact();
                maceInstant = null;
            }
        }
    }
}
