using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketUpgradeTilesUI : MonoBehaviour {
    public MusketUpgradeTile[] musketUpgradeTiles;

    void updateTiles()
    {
        foreach(MusketUpgradeTile tile in musketUpgradeTiles)
        {
            if (PlayerUpgrades.musketUpgrades.Contains(tile.upgradeID))
            {
                setActive(tile);
            }
            else
            {
                setUnActive(tile);
            }
        }
    }

	void Awake () {
        musketUpgradeTiles = GetComponentsInChildren<MusketUpgradeTile>();
        updateTiles();
	}

    void setActive(MusketUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }
    
    void setUnActive(MusketUpgradeTile tile)
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
