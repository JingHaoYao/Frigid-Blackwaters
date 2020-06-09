using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlaisesTome : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    public GameObject fireBall, summonEffect;
    GameObject fireBallInstant;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true)
        {
            if(fireBallInstant == null && Time.timeScale == 1)
            {
                fireBallInstant = Instantiate(fireBall, playerScript.transform.position, Quaternion.identity);
                Instantiate(summonEffect, playerScript.transform.position, Quaternion.identity);
            }
        }
        else
        {
            if(fireBallInstant != null)
            {
                Destroy(fireBallInstant);
            }
        }
    }

    public override void cameraMovedPosition(Vector3 currentPosition)
    {
        if(fireBallInstant != null)
        {
            fireBallInstant.transform.position = PlayerProperties.cursorPosition;
        }
    }
}
