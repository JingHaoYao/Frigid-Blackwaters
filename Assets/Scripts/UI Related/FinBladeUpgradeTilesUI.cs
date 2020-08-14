using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinBladeUpgradeTilesUI : MonoBehaviour
{
    public FinBladeUpgradeTile[] FinBladeUpgradeTiles;

    void updateTiles()
    {
        foreach (FinBladeUpgradeTile tile in FinBladeUpgradeTiles)
        {
            if (PlayerUpgrades.finBladeUpgrades.Contains(tile.upgradeID))
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
        FinBladeUpgradeTiles = GetComponentsInChildren<FinBladeUpgradeTile>();
        updateTiles();
    }

    void setActive(FinBladeUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(FinBladeUpgradeTile tile)
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
