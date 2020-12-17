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
    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void Start () {
        playerShip = GameObject.Find("PlayerShip");
        dialogueUI = FindObjectOfType<DungeonEntryDialogueManager>().dialogueUI;
        dialogueBlackOverlay = FindObjectOfType<DungeonEntryDialogueManager>().dialogueBlackOverlay;
        text = this.GetComponent<Text>();
        obstacleToolTip = GameObject.Find("PlayerShip").GetComponent<PlayerScript>().obstacleToolTip;
        SetInventoryAnimation();
	}

    void SetInventoryAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(0, -400, 0), new Vector3(0, -180, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(0, -180, 0), new Vector3(0, -400, 0), 0.25f);
    }

    public void PlayToolTipCloseAnimation()
    {
        menuSlideAnimation.PlayEndingAnimation(obstacleToolTip, () => { obstacleToolTip.SetActive(false); toolTipActive = false; });
    }

    public void PlayOpenToolTipAnimation()
    {
        obstacleToolTip.SetActive(true);
        toolTipActive = true;
        menuSlideAnimation.PlayOpeningAnimation(obstacleToolTip);
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
                    dialogueUI.LoadDialogueUI(examineDialogue, 0);
                }
                else
                {
                    if (obstacleToolTip.activeSelf == true)
                    {
                        if (toolTipActive)
                        {
                            PlayToolTipCloseAnimation();
                            PlayerProperties.playerScript.removeRootingObject();
                            PlayerProperties.playerScript.windowAlreadyOpen = false;
                        }
                    }
                    else
                    {
                        if (!toolTipActive)
                        {
                            PlayOpenToolTipAnimation();
                            obstacleToolTip.GetComponentInChildren<Text>().text = text.text;
                            PlayerProperties.playerScript.addRootingObject();
                            PlayerProperties.playerScript.windowAlreadyOpen = true;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (obstacleToolTip.activeSelf == true && toolTipActive)
                {
                    PlayToolTipCloseAnimation();
                    PlayerProperties.playerScript.windowAlreadyOpen = false;
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
