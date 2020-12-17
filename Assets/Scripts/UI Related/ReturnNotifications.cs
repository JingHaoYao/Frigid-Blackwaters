using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReturnNotifications : MonoBehaviour
{
    PlayerScript playerScript;
    public List<DialogueSet> dialoguesToDisplay;

    public DialogueUI dialogueUI;

    bool notificationsClosed = false;
    bool buildingUnlocked = false;

    public GameObject mapSymbol;

    [SerializeField] List<Image> returningArtifactsEquipped;
    [SerializeField] List<Image> inventoryItems;
    [SerializeField] Text goldDeposited;
    [SerializeField] Image playerSurvived, playerDead;
    [SerializeField] Image missionIcon;
    [SerializeField] Text missionStatus;
    [SerializeField] GameObject redX;
    [SerializeField] List<Image> missionItemRewards;
    [SerializeField] Text skillPointRewards;
    [SerializeField] Text goldRewards;

    [SerializeField] GameObject inventoryDisplay;
    [SerializeField] GameObject missionDisplay;

    [SerializeField] AudioSource pageTurn1, pageTurn2;
    [SerializeField] List<Image> sideRunes;

    UnityAction<int> midDialogueAction;

    MenuSlideAnimation inventoryPanelSlideAnimation = new MenuSlideAnimation();
    MenuSlideAnimation missionPanelSlideAnimation = new MenuSlideAnimation();
    MenuSlideAnimation labelSlideAnimation = new MenuSlideAnimation();


    [SerializeField] GameObject pressEsc;

    int goldAmountToDisplay = 0;
    int goldRewardAmount = 0;
    string missionStatusToDisplay = "";
    string skillPointsMessageToDisplay = "";

    void SetMenuAnimations()
    {
        inventoryPanelSlideAnimation.SetOpenAnimation(new Vector3(-950, -52, 0), new Vector3(-208, -52, 0), 0.5f);
        inventoryPanelSlideAnimation.SetCloseAnimation(new Vector3(-208, -52, 0), new Vector3(-950, -52, 0), 0.5f);

        missionPanelSlideAnimation.SetOpenAnimation(new Vector3(950, 27, 0), new Vector3(269, 27, 0), 0.5f);
        missionPanelSlideAnimation.SetCloseAnimation(new Vector3(269, 27, 0), new Vector3(950, 27, 0), 0.5f);

        labelSlideAnimation.SetOpenAnimation(new Vector3(0, -585, 0), new Vector3(0, -262, 0), 0.5f);
        labelSlideAnimation.SetCloseAnimation(new Vector3(0, -262, 0), new Vector3(0, -585, 0), 0.5f);
    }

    public void setMidDialogueAction(UnityAction<int> midDialogueAction)
    {
        this.midDialogueAction = midDialogueAction;
    }

    public void updateGoldDeposited(int goldAmount)
    {
        goldAmountToDisplay = goldAmount;
        goldDeposited.text = "0";
    }

    public void updatePlayerStatus(bool dead)
    {
        playerSurvived.enabled = !dead;
        playerDead.enabled = dead;
    }

    public void updateRewards(int goldAmount, int skillPointAmount, GameObject[] itemRewards, Sprite missionIcon, bool failedMission = false, bool missionIsCompleted = false)
    {
        this.missionIcon.sprite = missionIcon;
        
        for(int i = 0; i < 25; i++)
        {
            if(i < PlayerProperties.playerInventory.itemList.Count)
            {
                inventoryItems[i].sprite = PlayerProperties.playerInventory.itemList[i].GetComponent<DisplayItem>().displayIcon;
                inventoryItems[i].preserveAspect = true;
            }
            else
            {
                this.inventoryItems[i].enabled = false;
            }
        }

        for(int i = 0; i < 3; i++)
        {
            if(i < PlayerProperties.playerArtifacts.activeArtifacts.Count)
            {
                returningArtifactsEquipped[i].sprite = PlayerProperties.playerArtifacts.activeArtifacts[i].GetComponent<DisplayItem>().displayIcon;
                returningArtifactsEquipped[i].preserveAspect = true;
            }
            else
            {
                returningArtifactsEquipped[i].enabled = false;
            }
        }

        missionStatus.text = "";

        if(failedMission == true)
        {
            redX.SetActive(true);
            foreach(Image image in missionItemRewards)
            {
                image.enabled = false;
            }
            skillPointRewards.enabled = false;
            goldRewards.enabled = false;
            missionStatusToDisplay = "Boss Undefeated";
            return;
        }
        else if(missionIsCompleted == true)
        {
            redX.SetActive(true);
            foreach (Image image in missionItemRewards)
            {
                image.enabled = false;
            }
            skillPointRewards.enabled = false;
            goldRewards.enabled = false;
            missionStatusToDisplay = "Boss Already Defeated";
            return;
        }
        else
        {
            redX.SetActive(false);
            missionStatusToDisplay = "Boss Defeated";
            for(int i = 0; i < 3; i++)
            {
                if(i < itemRewards.Length)
                {
                    missionItemRewards[i].sprite = itemRewards[i].GetComponent<DisplayItem>().displayIcon;
                    missionItemRewards[i].preserveAspect = true;
                }
                else
                {
                    missionItemRewards[i].enabled = false;
                }
            }

            if (skillPointAmount > 0) {
                skillPointsMessageToDisplay = skillPointAmount.ToString() + " Skill Point Granted";
                skillPointRewards.text = "";
            }
            else
            {
                skillPointsMessageToDisplay = "No Skill Points Rewarded";
                skillPointRewards.text = "";
            }

            goldRewards.text = "";
            goldRewardAmount = goldAmount;
        }
    }

    void Start()
    {
        playerScript = PlayerProperties.playerScript;
        SetMenuAnimations();
    }

    public void closeNotifications()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        this.GetComponent<Image>().enabled = false;

        notificationsClosed = true;

        StartCoroutine(dialogueLoop());
    }

    public void activateNotifications()
    {
        StartCoroutine(notificationAnimations());
    }

    IEnumerator goldClimbAnimation(int goldAmount, Text text)
    {
        text.text = "0";
        int currentGoldAmount = 0;

        while(currentGoldAmount < goldAmount)
        {
            if (goldAmount - currentGoldAmount > 1000)
            {
                currentGoldAmount += 1000;
            }
            else if (goldAmount - currentGoldAmount > 100)
            {
                currentGoldAmount += 100;
            }
            else if(goldAmount - currentGoldAmount > 10)
            {
                currentGoldAmount += 10;
            }
            else
            {
                currentGoldAmount += 1;
            }
            text.text = currentGoldAmount.ToString();
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator animateText(string s, Text text)
    {
        int charIndex = 0;
        foreach (char c in s)
        {
            charIndex++;
            text.text = s.Substring(0, charIndex);
            if (c != ' ')
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    IEnumerator tweenAlpha(Image image, float duration)
    {
        float increment = 1 / duration;
        float period = 0;

        while(period < duration)
        {
            period += Time.deltaTime;
            image.color = new Color(1, 1, 1, period * increment);
            yield return null;
        }

        image.color = Color.white;
    }

    IEnumerator notificationAnimations()
    {
        yield return new WaitForEndOfFrame();
        PlayerProperties.playerScript.windowAlreadyOpen = true;
        PlayerProperties.playerScript.playerDead = true;

        inventoryDisplay.transform.localPosition = new Vector3(-950, -68, 0);
        missionDisplay.transform.localPosition = new Vector3(950, 11, 0);

        playerSurvived.color = new Color(1, 1, 1, 0);
        playerDead.color = new Color(1, 1, 1, 0);
        foreach(Image image in sideRunes)
        {
            image.color = new Color(1, 1, 1, 0);
            StartCoroutine(tweenAlpha(image, 0.5f));
        }
        StartCoroutine(tweenAlpha(playerDead, 0.5f));
        StartCoroutine(tweenAlpha(playerSurvived, 0.5f));

        labelSlideAnimation.PlayOpeningAnimation(pressEsc);

        yield return new WaitForSeconds(0.5f);

        inventoryPanelSlideAnimation.PlayOpeningAnimation(inventoryDisplay);
        pageTurn2.Play();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(goldClimbAnimation(goldAmountToDisplay, goldDeposited));

        missionPanelSlideAnimation.PlayOpeningAnimation(missionDisplay);
        pageTurn1.Play();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(animateText(missionStatusToDisplay, missionStatus));

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(goldClimbAnimation(goldRewardAmount, goldRewards));
        StartCoroutine(animateText(skillPointsMessageToDisplay, skillPointRewards));
    }

    IEnumerator endingAnimation()
    {
        inventoryPanelSlideAnimation.PlayEndingAnimation(inventoryDisplay, () => { });
        missionPanelSlideAnimation.PlayEndingAnimation(missionDisplay, () => { });
        labelSlideAnimation.PlayEndingAnimation(pressEsc, () => { });
        foreach (Image image in sideRunes)
        {
            StartCoroutine(tweenAlphaToZero(image, 0.5f, 1));
        }
        StartCoroutine(tweenAlphaToZero(playerDead, 0.5f, 1));
        StartCoroutine(tweenAlphaToZero(playerSurvived, 0.5f, 1));

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(tweenAlphaToZero(GetComponent<Image>(), 1f, 0.7f));
        playerScript.windowAlreadyOpen = false;
        playerScript.playerDead = false;
        StartCoroutine(dialogueLoop());
    }

    IEnumerator tweenAlphaToZero(Image image, float duration, float start)
    {
        float increment = start / duration;
        float timer = duration;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            image.color = new Color(1, 1, 1, timer * increment);
            yield return null;
        }

        image.color = new Color(1, 1, 1, 0);
    }

    IEnumerator dialogueLoop()
    {
        while (true)
        {
            if (dialoguesToDisplay.Count > 0)
            {
                if (dialogueUI.gameObject.activeSelf == false)
                {
                    dialogueUI.LoadDialogueUI(dialoguesToDisplay[0], 0.001f);
                    dialoguesToDisplay.Remove(dialoguesToDisplay[0]);

                    if (midDialogueAction != null)
                    {
                        midDialogueAction.Invoke(dialoguesToDisplay.Count);
                    }

                    buildingUnlocked = true;
                }
            }

            if (dialoguesToDisplay.Count == 0 && buildingUnlocked)
            {
                if (dialogueUI.gameObject.activeSelf == false)
                {
                    mapSymbol.SetActive(true);
                    this.gameObject.SetActive(false);
                }
            }

            yield return null;
        }
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && notificationsClosed == false)
        {
            notificationsClosed = true;
            StartCoroutine(endingAnimation());
        }
    }
}
