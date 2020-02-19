using UnityEngine;
using UnityEngine.UI;

public class GlaiveLauncherUpgradeTileUI : MonoBehaviour
{
    public GlaiveLauncherUpgradeTile[] GlaiveLauncherUpgradeTiles;

    void updateTiles()
    {
        foreach (GlaiveLauncherUpgradeTile tile in GlaiveLauncherUpgradeTiles)
        {
            if (PlayerUpgrades.glaiveLauncherUpgrades.Contains(tile.upgradeID))
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
        GlaiveLauncherUpgradeTiles = GetComponentsInChildren<GlaiveLauncherUpgradeTile>();
        updateTiles();
    }

    void setActive(GlaiveLauncherUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(GlaiveLauncherUpgradeTile tile)
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
