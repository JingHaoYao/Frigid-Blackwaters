using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaliciousFleshHook : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
    }

    // Whenever the player enters a previously unentered room
    public override void exploredNewRoom(int whatRoomType) { PlayerProperties.playerScript.dealDamageToShip(50, this.gameObject); }
}
