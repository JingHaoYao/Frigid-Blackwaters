using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingCannonUpgradeTilesUI : MonoBehaviour
{
    public RevolvingCannonUpgradeTile[] RevolvingCannonUpgradeTiles;

    void updateTiles()
    {
        foreach (RevolvingCannonUpgradeTile tile in RevolvingCannonUpgradeTiles)
        {
            if (PlayerUpgrades.revolvingCannonUpgrades.Contains(tile.upgradeID))
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
        RevolvingCannonUpgradeTiles = GetComponentsInChildren<RevolvingCannonUpgradeTile>();
        updateTiles();
    }

    void setActive(RevolvingCannonUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(RevolvingCannonUpgradeTile tile)
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
