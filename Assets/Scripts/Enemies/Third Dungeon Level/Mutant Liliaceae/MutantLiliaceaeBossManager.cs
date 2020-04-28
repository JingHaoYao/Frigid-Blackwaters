using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantLiliaceaeBossManager : BossManager
{
    public BlackOverlay fadeWindow;
    public Camera camera;
    public GameObject mutantLiliaceaeBoss;

    public void InitiateBossFromCheckpoint()
    {
        StartCoroutine(movePlayerToBossArea());
    }

    IEnumerator movePlayerToBossArea()
    {
        fadeWindow.transition();
        PlayerProperties.playerScript.playerDead = true;
        yield return new WaitForSeconds(1f);
        PlayerProperties.playerShip.transform.position = new Vector3(1600, -6);
        camera.transform.position = new Vector3(1600, 0);
        yield return new WaitForSeconds(0.5f);
        mutantLiliaceaeBoss.SetActive(true);
    }
}
