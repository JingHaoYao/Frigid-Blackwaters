using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadshotUpgradeTilesUI : MonoBehaviour {
    public SpreadshotUpgradeTile[] SpreadshotUpgradeTiles;

    void updateTiles()
    {
        foreach (SpreadshotUpgradeTile tile in SpreadshotUpgradeTiles)
        {
            if (PlayerUpgrades.spreadshotUpgrades.Contains(tile.upgradeID))
            {
                setActive(tile);
            }
            else
            {
                setUnActive(tile);
            }
        }
    }

    void Start()
    {
        SpreadshotUpgradeTiles = GetComponentsInChildren<SpreadshotUpgradeTile>();
        updateTiles();
    }

    void setActive(SpreadshotUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(SpreadshotUpgradeTile tile)
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
