using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluxShrineUpgradeTilesUI : MonoBehaviour
{
    public PolluxShrineUpgradeTile[] PolluxShrineUpgradeTiles;

    void updateTiles()
    {
        foreach (PolluxShrineUpgradeTile tile in PolluxShrineUpgradeTiles)
        {
            if (PlayerUpgrades.polluxShrineUpgrades.Contains(tile.upgradeID))
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
        PolluxShrineUpgradeTiles = GetComponentsInChildren<PolluxShrineUpgradeTile>();
        updateTiles();
    }

    void setActive(PolluxShrineUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(PolluxShrineUpgradeTile tile)
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
