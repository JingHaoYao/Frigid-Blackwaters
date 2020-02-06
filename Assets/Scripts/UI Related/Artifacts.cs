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

	void Awake() {
        playerScript = GetComponent<PlayerScript>();
        activeSlots = activeSlotsParent.GetComponentsInChildren<ActiveSlot>();
        artifactSlots = artifactSlotsParent.GetComponentsInChildren<ArtifactSlot>();
	}
	
	void LateUpdate () {
        if (chestUI.activeSelf == false && shopUI.activeSelf == false)
        {
            if (artifactsUI.activeSelf == false)
            {
                if (GetComponent<PlayerScript>().playerDead == false)
                {
                    if (Input.GetKeyDown(KeyCode.I) && playerScript.windowAlreadyOpen == false)
                    {
                        playerScript.windowAlreadyOpen = true;
                        UpdateUI();
                        artifactsUI.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I))
                {
                    playerScript.windowAlreadyOpen = false;
                    artifactsUI.SetActive(false);
                    Time.timeScale = 1;
                }
            }
        }

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

        float speedBonus = 0;
        float defenseBonus = 0;
        int attackBonus = 0;
        int healthBonus = 0;
        int periodicHealing = 0;
        int bonusArtifactChance = 0;
        int bonusGold = 0;
        for(int i = 0; i < activeArtifacts.Count; i++)
        {
                speedBonus += activeArtifacts[i].GetComponent<ArtifactBonus>().speedBonus;
                defenseBonus += activeArtifacts[i].GetComponent<ArtifactBonus>().defenseBonus;
                attackBonus += activeArtifacts[i].GetComponent<ArtifactBonus>().attackBonus;
                healthBonus += activeArtifacts[i].GetComponent<ArtifactBonus>().healthBonus;
                periodicHealing += activeArtifacts[i].GetComponent<ArtifactBonus>().periodicHealing;
                bonusArtifactChance += activeArtifacts[i].GetComponent<ArtifactBonus>().artifactChanceBonus;
                bonusGold += bonusArtifactChance += activeArtifacts[i].GetComponent<ArtifactBonus>().goldBonus;
        }

        PlayerScript playerScript = this.gameObject.GetComponent<PlayerScript>();
        playerScript.healthBonus = healthBonus;
        playerScript.defenseBonus = defenseBonus;
        playerScript.attackBonus = attackBonus;
        playerScript.speedBonus = speedBonus;
        playerScript.periodicHealing = periodicHealing;
        Chest.bonusArtifactChance = bonusArtifactChance;
        Chest.bonusGold = bonusGold;
    }
}
