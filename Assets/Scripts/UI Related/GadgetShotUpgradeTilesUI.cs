using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetShotUpgradeTilesUI : MonoBehaviour
{
    public GadgetShotUpgradeTile[] GadgetShotUpgradeTiles;

    void updateTiles()
    {
        foreach (GadgetShotUpgradeTile tile in GadgetShotUpgradeTiles)
        {
            if (PlayerUpgrades.gadgetShotUpgrades.Contains(tile.upgradeID))
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
        GadgetShotUpgradeTiles = GetComponentsInChildren<GadgetShotUpgradeTile>();
        updateTiles();
    }

    void setActive(GadgetShotUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(GadgetShotUpgradeTile tile)
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
