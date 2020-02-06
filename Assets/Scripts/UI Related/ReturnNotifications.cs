using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnNotifications : MonoBehaviour
{
    PlayerScript playerScript;
    public GameObject notificationBar;
    public List<DialogueSet> dialoguesToDisplay;

    public DialogueUI dialogueUI;
    public GameObject blackOverlay;

    bool notificationsClosed = false;
    bool buildingUnlocked = false;

    public GameObject mapSymbol;

    public Text missionAlreadyCompleted, missionFailed;
    public GameObject rewards;
    public GameObject itemRewardsUI;

    public void updateRewards(int goldAmount, int skillPointAmount, GameObject[] itemRewards, bool failedMission = false, bool missionIsCompleted = false)
    {
        if(failedMission == true)
        {
            rewards.SetActive(false);
            missionFailed.gameObject.SetActive(true);
            return;
        }
        else if(missionIsCompleted == true)
        {
            rewards.SetActive(false);
            missionAlreadyCompleted.gameObject.SetActive(true);
            return;
        }
        else
        {
            rewards.GetComponentsInChildren<Image>()[0].GetComponentInChildren<Text>().text = goldAmount.ToString();
            rewards.GetComponentsInChildren<Image>()[1].GetComponentInChildren<Text>().text = skillPointAmount.ToString();
            for (int i = 0; i < 3; i++) {
                if (i < itemRewards.Length)
                {
                    itemRewardsUI.GetComponentsInChildren<Image>()[i].GetComponentsInChildren<Image>()[1].sprite = itemRewards[i].GetComponent<DisplayItem>().displayIcon;
                }
                else
                {
                    itemRewardsUI.GetComponentsInChildren<Image>()[2 * i].GetComponentsInChildren<Image>()[1].enabled = false;
                }
            }
        }
    }

    void Awake()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        if (MiscData.finishedTutorial == false || MiscData.missionID == null)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            this.GetComponent<Image>().enabled = false;
            notificationsClosed = true;
        }
        else
        {
            playerScript.windowAlreadyOpen = true;
            playerScript.playerDead = true;
            MiscData.finishedMission = false;
            MiscData.missionID = null;
        }
    }

    void LateUpdate()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)) && notificationsClosed == false)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            playerScript.windowAlreadyOpen = false;
            playerScript.playerDead = false;
            notificationsClosed = true;
            this.GetComponent<Image>().enabled = false;
        }

        if(notificationsClosed == true && dialoguesToDisplay.Count > 0)
        {
            if(dialogueUI.gameObject.activeSelf == false)
            {
                dialogueUI.targetDialogue = dialoguesToDisplay[0];
                dialoguesToDisplay.Remove(dialoguesToDisplay[0]);
                dialogueUI.gameObject.SetActive(true);
                blackOverlay.SetActive(true);
                buildingUnlocked = true;
            }
        }

        if(dialoguesToDisplay.Count == 0 && buildingUnlocked)
        {
            if(dialogueUI.gameObject.activeSelf == false)
            {
                mapSymbol.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}
