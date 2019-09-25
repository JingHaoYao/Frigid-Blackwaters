using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySafeUpgradeTilesUI : MonoBehaviour {
    public InventorySafeUpgradeTile[] InventorySafeUpgradeTiles;

    void updateTiles()
    {
        foreach (InventorySafeUpgradeTile tile in InventorySafeUpgradeTiles)
        {
            if(tile.isInventoryTile == true)
            {
                if (PlayerUpgrades.inventoryUpgrades.Contains(tile.upgradeID))
                {
                    setActive(tile);
                }
                else
                {
                    setUnActive(tile);
                }
            }
            else
            {
                if (PlayerUpgrades.safeUpgrades.Contains(tile.upgradeID))
                {
                    setActive(tile);
                }
                else
                {
                    setUnActive(tile);
                }
            }
        }
    }

    void Start()
    {
        InventorySafeUpgradeTiles = GetComponentsInChildren<InventorySafeUpgradeTile>();
        updateTiles();
    }

    void setActive(InventorySafeUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(InventorySafeUpgradeTile tile)
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
