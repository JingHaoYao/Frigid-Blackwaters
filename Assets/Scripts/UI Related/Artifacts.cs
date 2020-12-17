using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifacts : MonoBehaviour {
    public GameObject artifactsUI, chestUI, shopUI;
    public ArtifactSlot[] artifactSlots;
    public ActiveSlot[] activeSlots;
    public List<GameObject> activeArtifacts = new List<GameObject>();
    public Transform artifactSlotsParent;
    public Transform activeSlotsParent;
    public int killMax = 20;
    public int numKills = 0;
    PlayerScript playerScript;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetArtifactsAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(-243, -585, 0), new Vector3(-243, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(-243, 0, 0), new Vector3(-243, -585, 0), 0.25f);
    }

    public void PlayEnteringAnimation()
    {
        menuSlideAnimation.PlayOpeningAnimation(artifactsUI);
    }

    void Awake() {
        playerScript = GetComponent<PlayerScript>();
        activeSlots = activeSlotsParent.GetComponentsInChildren<ActiveSlot>();
        artifactSlots = artifactSlotsParent.GetComponentsInChildren<ArtifactSlot>();
        PlayerProperties.playerArtifacts = this;
        SetArtifactsAnimation();
	}

    public void OpenArtifacts()
    {
        playerScript.windowAlreadyOpen = true;
        UpdateUI();
        artifactsUI.SetActive(true);
        PlayEnteringAnimation();
        Time.timeScale = 0;
    }

    public void CloseArtifacts()
    {
        playerScript.windowAlreadyOpen = false;
        menuSlideAnimation.PlayEndingAnimation(artifactsUI, () => artifactsUI.SetActive(false));
        Time.timeScale = 1;
    }
	
	void LateUpdate () {

        if(numKills > 20)
        {
            numKills = 20;
        }

        if(numKills < 0)
        {
            numKills = 0;
        }

        PlayerItems.numberArtifactKills = numKills;
    }

    public void UpdateStats()
    {
        float speedBonus = 0;
        float defenseBonus = 0;
        int attackBonus = 0;
        int healthBonus = 0;
        int periodicHealing = 0;
        int bonusArtifactChance = 0;
        int bonusGold = 0;
        for (int i = 0; i < activeArtifacts.Count; i++)
        {
            ArtifactBonus artifactBonus = activeArtifacts[i].GetComponent<ArtifactBonus>();
            speedBonus += artifactBonus.speedBonus;
            defenseBonus += artifactBonus.defenseBonus;
            attackBonus += artifactBonus.attackBonus;
            healthBonus += artifactBonus.healthBonus;
            periodicHealing += artifactBonus.periodicHealing;
            bonusArtifactChance += artifactBonus.artifactChanceBonus;
            bonusGold += bonusArtifactChance += artifactBonus.goldBonus;
        }

        PlayerScript playerScript = PlayerProperties.playerScript;
        playerScript.healthBonus = healthBonus;
        playerScript.defenseBonus = defenseBonus;
        playerScript.attackBonus = attackBonus;
        playerScript.speedBonus = speedBonus;
        playerScript.periodicHealing = periodicHealing;
        Chest.bonusArtifactChance = bonusArtifactChance;
        Chest.bonusGold = bonusGold;

        foreach (GameObject artifact in activeArtifacts)
        {
            artifact.GetComponent<ArtifactEffect>()?.updatedArtifactStats();
        }

        playerScript.CheckAndUpdateHealth();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < artifactSlots.Length; i++)
        {
            if (i < activeArtifacts.Count)
            {
                artifactSlots[i].addSlot(activeArtifacts[i].GetComponent<DisplayItem>());
                PlayerItems.activeArtifactsIDs[i] = activeArtifacts[i].name;
            }
            else
            {
                artifactSlots[i].deleteSlot();
                if (PlayerItems.activeArtifactsIDs[i] != null)
                {
                    PlayerItems.activeArtifactsIDs[i] = null;
                }
            }
        }

        int count = 1;
        for (int k = 0; k < activeSlots.Length; k++)
        {
            if (count <= activeArtifacts.Count && activeArtifacts[k].GetComponent<DisplayItem>().hasActive == true)
            {
                activeSlots[k].addSlot(activeArtifacts[k].GetComponent<DisplayItem>());
                activeArtifacts[k].GetComponent<DisplayItem>().isEquipped = true;
                activeArtifacts[k].GetComponent<DisplayItem>().whichSlot = k;
            }
            else
            {
                activeSlots[k].deleteSlot();
            }
            count++;
        }

        UpdateStats();
    }
}
