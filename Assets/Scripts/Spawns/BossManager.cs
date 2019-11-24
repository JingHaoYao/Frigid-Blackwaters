using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public string tiedMissionID;
    public Vector3 shipMoveToPosition;
    public Vector3 camPosition;
    public GameObject roomReveal;
    // if a piece of dialogue needs to be played before the boss fight
    // some bosses need a story checkpoint which allows the player to move around to choose whether to attempt the boss or not
    public StoryCheckpoint beginningCheckPoint;

    // the story checkpoint after beating the boss fight
    public StoryCheckpoint finishedBossCheckPoint;

    public GameObject prelimRoom;
    public GameObject centerRoom;

    public void startBossSequence(int whichSide)
    {
        if (beginningCheckPoint != null)
        {
            Camera.main.transform.position = beginningCheckPoint.cameraPosition;
            FindObjectOfType<PlayerScript>().transform.position = beginningCheckPoint.playerShipPosition;
            beginningCheckPoint.startUpDialogue();
        }
        else
        {
            adjustRoomOrientation(whichSide);
            Camera.main.transform.position = camPosition;
            FindObjectOfType<PlayerScript>().transform.position = shipMoveToPosition;
            Instantiate(roomReveal, camPosition, Quaternion.identity);
            StartCoroutine(adjustPlayer());
        }
    }

    void adjustRoomOrientation(int whichSide)
    {
        // whichSide (which side the player came from)
        // 1 - right
        // 2 - top
        // 3 - left
        // 4 - bottom

        if (whichSide == 1)
        {
            prelimRoom.transform.rotation = Quaternion.Euler(0, 0, 90);
            centerRoom.transform.rotation = Quaternion.Euler(0, 0, 90);
            prelimRoom.transform.position = centerRoom.transform.position + new Vector3(20, 0, 0);
            shipMoveToPosition = centerRoom.transform.position + new Vector3(26.5f, 0, 0);
            camPosition = centerRoom.transform.position + new Vector3(20, 0, 0);
        }
        else if (whichSide == 2)
        {
            prelimRoom.transform.rotation = Quaternion.Euler(0, 0, 180);
            centerRoom.transform.rotation = Quaternion.Euler(0, 0, 180);
            prelimRoom.transform.position = centerRoom.transform.position + new Vector3(0, 20, 0);
            shipMoveToPosition = centerRoom.transform.position + new Vector3(0, 26.5f, 0);
            camPosition = centerRoom.transform.position + new Vector3(0, 20, 0);
        }
        else if (whichSide == 3)
        {
            prelimRoom.transform.rotation = Quaternion.Euler(0, 0, 270);
            centerRoom.transform.rotation = Quaternion.Euler(0, 0, 270);
            prelimRoom.transform.position = centerRoom.transform.position + new Vector3(-20, 0, 0);
            shipMoveToPosition = centerRoom.transform.position + new Vector3(-26.5f, 0, 0);
            camPosition = centerRoom.transform.position + new Vector3(-20, 0, 0);
        }
        else
        {
            prelimRoom.transform.rotation = Quaternion.Euler(0, 0, 0);
            centerRoom.transform.rotation = Quaternion.Euler(0, 0, 0);
            prelimRoom.transform.position = centerRoom.transform.position + new Vector3(0, -20, 0);
            shipMoveToPosition = centerRoom.transform.position + new Vector3(0, -26.5f, 0);
            camPosition = centerRoom.transform.position + new Vector3(0, -20, 0);
        }   
    }

    IEnumerator adjustPlayer()
    {
        FindObjectOfType<PlayerScript>().shipRooted = true;
        yield return new WaitForSeconds(0.2f);
        FindObjectOfType<PlayerScript>().shipRooted = false;
    }

    public void bossBeaten(string nameID, float delay)
    {
        FindObjectOfType<MissionManager>().finishedMission();
        if (!MiscData.bossesDefeated.Contains(nameID))
        {
            MiscData.bossesDefeated.Add(nameID);
        }

        StartCoroutine(delayCheckPoint(delay));
    }

    IEnumerator delayCheckPoint(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (!MiscData.completedStoryDialogues.Contains(finishedBossCheckPoint.dialogueName))
        {
            StartCoroutine(goToCheckPoint());
        }
        else
        {
            FindObjectOfType<PauseMenu>().loadHub();
        }
    }

    IEnumerator goToCheckPoint()
    {
        FindObjectOfType<BlackOverlay>().transition();
        yield return new WaitForSeconds(1f);
        Camera.main.transform.position = finishedBossCheckPoint.cameraPosition;
        FindObjectOfType<PlayerScript>().transform.position = finishedBossCheckPoint.playerShipPosition;
        finishedBossCheckPoint.startUpDialogue();
    }
}
