using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgradeTilesUI : MonoBehaviour {
    public HullUpgradeTile[] HullUpgradeTiles;

    void updateTiles()
    {
        foreach (HullUpgradeTile tile in HullUpgradeTiles)
        {
            if (PlayerUpgrades.hullUpgrades.Contains(tile.upgradeID))
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
        HullUpgradeTiles = GetComponentsInChildren<HullUpgradeTile>();
        updateTiles();
    }

    void setActive(HullUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(HullUpgradeTile tile)
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
