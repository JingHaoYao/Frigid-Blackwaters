using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GassyMushroom : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject gasCloud;


    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
    }

    // whenever the player dashes
    public override void playerDashed() {
        GameObject gas = Instantiate(gasCloud, PlayerProperties.playerShipPosition, Quaternion.Euler(0, 0, PlayerProperties.playerScript.whatAngleTraveled + 90));
    }
}
