using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremorMakerUpgradeTilesUI : MonoBehaviour
{
    public TremorMakerUpgradeTile[] TremorMakerUpgradeTiles;

    void updateTiles()
    {
        foreach (TremorMakerUpgradeTile tile in TremorMakerUpgradeTiles)
        {
            if (PlayerUpgrades.tremorMakerUpgrades.Contains(tile.upgradeID))
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
        TremorMakerUpgradeTiles = GetComponentsInChildren<TremorMakerUpgradeTile>();
        updateTiles();
    }

    void setActive(TremorMakerUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(TremorMakerUpgradeTile tile)
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
