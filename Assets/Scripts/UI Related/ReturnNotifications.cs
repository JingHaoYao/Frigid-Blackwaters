using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    private NotificationBell dialogueNotifications;

    UnityAction<int> midDialogueAction;

    public void setMidDialogueAction(UnityAction<int> midDialogueAction)
    {
        this.midDialogueAction = midDialogueAction;
    }

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

    void Start()
    {
        playerScript = PlayerProperties.playerScript;
        NotificationBell[] notifications = notificationBar.GetComponentsInChildren<NotificationBell>();
        dialogueNotifications = notifications[0];
    }

    public NotificationBell dialogueNotification
    {
        get {
            return this.dialogueNotifications;
        }
    }

    public void closeNotifications()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        this.GetComponent<Image>().enabled = false;

        notificationsClosed = true;
    }

    public void activateNotifications()
    {
        PlayerProperties.playerScript.windowAlreadyOpen = true;
        PlayerProperties.playerScript.playerDead = true;
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
                dialogueUI.waitReveal = 0.001f;
                dialogueUI.gameObject.SetActive(true);
                dialoguesToDisplay.Remove(dialoguesToDisplay[0]);

                if(midDialogueAction != null)
                {
                    midDialogueAction.Invoke(dialoguesToDisplay.Count);
                }

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
