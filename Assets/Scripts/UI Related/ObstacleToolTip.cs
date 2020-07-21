using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleToolTip : MonoBehaviour {
    GameObject playerShip, spawnedIndicator;
    public GameObject obstacleToolTip, indicator;
    bool toolTipActive = false;
    Text text;
    public DialogueSet examineDialogue;
    DialogueUI dialogueUI;
    GameObject dialogueBlackOverlay;

	void Start () {
        playerShip = GameObject.Find("PlayerShip");
        dialogueUI = FindObjectOfType<DungeonEntryDialogueManager>().dialogueUI;
        dialogueBlackOverlay = FindObjectOfType<DungeonEntryDialogueManager>().dialogueBlackOverlay;
        text = this.GetComponent<Text>();
        obstacleToolTip = GameObject.Find("PlayerShip").GetComponent<PlayerScript>().obstacleToolTip;
	}

    void LateUpdate()
    {
        if ((Vector2.Distance(playerShip.transform.position, transform.position) < 3f || toolTipActive == true) && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true)
        {
            if(toolTipActive == false && dialogueUI.isActiveAndEnabled == false)
            {
                if (spawnedIndicator == null)
                {
                    spawnedIndicator = Instantiate(indicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                    spawnedIndicator.GetComponent<ExamineIndicator>().parentObject = this.gameObject;
                }
            }
            else
            {
                if(spawnedIndicator != null)
                {
                    Destroy(spawnedIndicator);
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (examineDialogue != null && !MiscData.completedExamineDialogues.Contains(examineDialogue.gameObject.name))
                {
                    dialogueUI.targetDialogue = examineDialogue;
                    dialogueUI.gameObject.SetActive(true);
                    dialogueBlackOverlay.SetActive(true);
                }
                else
                {
                    if (obstacleToolTip.activeSelf == true)
                    {
                        obstacleToolTip.SetActive(false);
                        toolTipActive = false;
                        PlayerProperties.playerScript.removeRootingObject();
                    }
                    else
                    {
                        toolTipActive = true;
                        obstacleToolTip.GetComponentInChildren<Text>().text = text.text;
                        obstacleToolTip.SetActive(true);
                        PlayerProperties.playerScript.addRootingObject();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (obstacleToolTip.activeSelf == true)
                {
                    obstacleToolTip.SetActive(false);
                    toolTipActive = false;
                    PlayerProperties.playerScript.removeRootingObject();
                }
            }
        }
        else
        {
            if(spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
        }
    }
}
