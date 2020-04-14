using UnityEngine;

public class PodFlyersUpgradeTilesUI : MonoBehaviour
{
    public PodFlyersUpgradeTile[] PodFlyersUpgradeTiles;

    void updateTiles()
    {
        foreach (PodFlyersUpgradeTile tile in PodFlyersUpgradeTiles)
        {
            if (PlayerUpgrades.podFlyersUpgrades.Contains(tile.upgradeID))
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
        PodFlyersUpgradeTiles = GetComponentsInChildren<PodFlyersUpgradeTile>();
        updateTiles();
    }

    void setActive(PodFlyersUpgradeTile tile)
    {
        tile.imageIcon.color = new Color(1, 1, 1, 1);
        tile.unlocked = true;
        tile.upgraded = true;
        tile.lockedIcon.SetActive(false);
    }

    void setUnActive(PodFlyersUpgradeTile tile)
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
