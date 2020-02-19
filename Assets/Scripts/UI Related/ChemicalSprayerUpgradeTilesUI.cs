using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalSprayerUpgradeTilesUI : MonoBehaviour
{
    public ChemicalSprayerUpgradeTile[] ChemicalSprayerUpgradeTiles;

    void updateTiles()
    {
        foreach (ChemicalSprayerUpgradeTile tile in ChemicalSprayerUpgradeTiles)
        {
            if (PlayerUpgrades.chemicalSprayerUpgrades.Contains(tile.upgradeID))
            {
                setActive(tile);
            }
            else
            {
                setUnActive(tile);
            }
        }
    }

    void Awake()
    {
        ChemicalSprayerUpgradeTiles = GetComponentsInChildren<ChemicalSprayerUpgradeTile>();
        updateTiles();
    }

    void setActive(ChemicalSprayerUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(ChemicalSprayerUpgradeTile tile)
    {
        tile.lockedIcon.SetActive(true);
        tile.imageIcon.color = new Color(1, 1, 1, 0.63f);
        tile.unlocked = false;
        tile.upgraded = false;
    }

    private void OnEnable()
    {
        updateTiles();
    }
}
