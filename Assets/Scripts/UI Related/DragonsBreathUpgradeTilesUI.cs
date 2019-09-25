using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonsBreathUpgradeTilesUI : MonoBehaviour {
    public DragonsBreathUpgradeTile[] DragonsBreathUpgradeTiles;

    void updateTiles()
    {
        foreach (DragonsBreathUpgradeTile tile in DragonsBreathUpgradeTiles)
        {
            if (PlayerUpgrades.dragonBreathUpgrades.Contains(tile.upgradeID))
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
        DragonsBreathUpgradeTiles = GetComponentsInChildren<DragonsBreathUpgradeTile>();
        updateTiles();
    }

    void setActive(DragonsBreathUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(DragonsBreathUpgradeTile tile)
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
