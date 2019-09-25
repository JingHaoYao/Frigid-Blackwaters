using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkUpgradeTilesUI : MonoBehaviour {
    public FireworkUpgradeTile[] FireworkUpgradeTiles;

    void updateTiles()
    {
        foreach (FireworkUpgradeTile tile in FireworkUpgradeTiles)
        {
            if (PlayerUpgrades.fireworkUpgrades.Contains(tile.upgradeID))
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
        FireworkUpgradeTiles = GetComponentsInChildren<FireworkUpgradeTile>();
        updateTiles();
    }

    void setActive(FireworkUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(FireworkUpgradeTile tile)
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
