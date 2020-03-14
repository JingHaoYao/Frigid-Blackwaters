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
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    // whenever the player dashes
    public override void playerDashed() {
        GameObject gas = Instantiate(gasCloud, playerScript.transform.position, Quaternion.Euler(0, 0, playerScript.angleEffect + 90));
    }
}
