using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoneSparkUpgradeTilesUI : MonoBehaviour
{
    public LoneSparkUpgradeTile[] LoneSparkUpgradeTiles;

    void updateTiles()
    {
        foreach (LoneSparkUpgradeTile tile in LoneSparkUpgradeTiles)
        {
            if (PlayerUpgrades.loneSparkUpgrades.Contains(tile.upgradeID))
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
        LoneSparkUpgradeTiles = GetComponentsInChildren<LoneSparkUpgradeTile>();
        updateTiles();
    }

    void setActive(LoneSparkUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(LoneSparkUpgradeTile tile)
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
