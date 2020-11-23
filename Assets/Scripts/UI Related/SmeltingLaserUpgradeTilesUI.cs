using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmeltingLaserUpgradeTilesUI : MonoBehaviour
{
    public SmeltingLaserUpgradeTile[] SmeltingLaserUpgradeTiles;

    void updateTiles()
    {
        foreach (SmeltingLaserUpgradeTile tile in SmeltingLaserUpgradeTiles)
        {
            if (PlayerUpgrades.smeltingLaserUpgrades.Contains(tile.upgradeID))
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
        SmeltingLaserUpgradeTiles = GetComponentsInChildren<SmeltingLaserUpgradeTile>();
        updateTiles();
    }

    void setActive(SmeltingLaserUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(SmeltingLaserUpgradeTile tile)
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
