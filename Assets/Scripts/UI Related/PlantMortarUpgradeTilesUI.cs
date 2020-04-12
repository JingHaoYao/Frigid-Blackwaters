using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMortarUpgradeTilesUI : MonoBehaviour
{
    public PlantMortarUpgradeTile[] PlantMortarUpgradeTiles;

    void updateTiles()
    {
        foreach (PlantMortarUpgradeTile tile in PlantMortarUpgradeTiles)
        {
            if (PlayerUpgrades.plantMortarUpgrades.Contains(tile.upgradeID))
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
        PlantMortarUpgradeTiles = GetComponentsInChildren<PlantMortarUpgradeTile>();
        updateTiles();
    }

    void setActive(PlantMortarUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(PlantMortarUpgradeTile tile)
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
