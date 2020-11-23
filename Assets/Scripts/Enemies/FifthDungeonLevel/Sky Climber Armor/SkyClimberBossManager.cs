using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyClimberBossManager : BossManager
{
    public MoveCameraNextRoom cameraScript;
    public SkyClimberArmor skyClimberArmor;
    public PlayerScript playerScript;
    public BlackOverlay fadeWindow;
    public GameObject skyClimberRooms;

    IEnumerator movePlayerToBossArea()
    {
        fadeWindow.transition();
        playerScript.playerDead = true;
        yield return new WaitForSeconds(1f);
        playerScript.transform.position = new Vector3(1500, -5);
        cameraScript.transform.position = new Vector3(1500, 0);
        cameraScript.GetComponent<Camera>().orthographicSize = 20;
        cameraScript.freeCam = true;
        skyClimberArmor.gameObject.SetActive(true);
    }

    // Called to transition the player over to the boss room
    public void startMovingPlayer()
    {
        skyClimberRooms.SetActive(true);
        StartCoroutine(movePlayerToBossArea());
    }
}
