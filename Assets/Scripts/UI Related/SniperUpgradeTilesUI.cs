using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperUpgradeTilesUI : MonoBehaviour
{
    public SniperUpgradeTile[] SniperUpgradeTiles;

    void updateTiles()
    {
        foreach (SniperUpgradeTile tile in SniperUpgradeTiles)
        {
            if (PlayerUpgrades.sniperUpgrades.Contains(tile.upgradeID))
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
        SniperUpgradeTiles = GetComponentsInChildren<SniperUpgradeTile>();
        updateTiles();
    }

    void setActive(SniperUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(SniperUpgradeTile tile)
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
