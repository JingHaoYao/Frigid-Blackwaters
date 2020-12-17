using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickWeaponMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> tileList;
    [SerializeField] private List<TrainingHouseWeaponIcon> trainingHouseWeaponIcons;
    public ShipWeaponTemplate[] allTemplates;

    [SerializeField] List<TutorialEntry> tutorialEntries;

    public void ShowTutorial()
    {
        PlayerProperties.tutorialWidgetMenu.Initialize(tutorialEntries);
    }

    private void Start()
    {
        SetTemplatesActive();
        SetTemplatesForWeaponIcons();
    }

    void SetTemplatesActive()
    {
        for(int i = 0; i < tileList.Count; i++)
        {
            tileList[i].transform.GetChild(0).gameObject.SetActive(MiscData.dungeonLevelUnlocked < allTemplates[i].whichLevelUnlock);
        }
    }

    void SetTemplatesForWeaponIcons()
    {
        foreach(TrainingHouseWeaponIcon icon in trainingHouseWeaponIcons)
        {
            icon.templates = allTemplates;
        }
    }
}
