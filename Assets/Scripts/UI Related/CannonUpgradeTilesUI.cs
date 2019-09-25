using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUpgradeTilesUI : MonoBehaviour {
    public CannonUpgradeTile[] CannonUpgradeTiles;

    void updateTiles()
    {
        foreach (CannonUpgradeTile tile in CannonUpgradeTiles)
        {
            if (PlayerUpgrades.cannonUpgrades.Contains(tile.upgradeID))
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
        CannonUpgradeTiles = GetComponentsInChildren<CannonUpgradeTile>();
        updateTiles();
    }

    void setActive(CannonUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(CannonUpgradeTile tile)
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
