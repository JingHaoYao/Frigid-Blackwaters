using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusketUpgradeTile : MonoBehaviour {
    public MusketUpgradeTile prevTile;
    public MusketUpgradeTile[] nextTiles;
    public GameObject lockedIcon;
    public Image imageIcon;
    public int skillPointsRequirement = 1;
    public string upgradeID;
    public bool upgraded = false;
    public bool unlocked = false;
    bool operationApplied = false;
    public bool noLongerUnlockable = false;
    public int whatLevelUnlockable = 2;

    private void Start()
    {
        if(upgraded == false)
        {
            imageIcon.color = new Color(1, 1, 1, 0.63f);
        }
        else
        {
            imageIcon.color = new Color(1, 1, 1, 1);
        }

        if(unlocked == false)
        {
            lockedIcon.SetActive(true);
        }
        else
        {
            lockedIcon.SetActive(false);
        }
    }

    private void Update()
    {
        if (MiscData.dungeonLevelUnlocked >= whatLevelUnlockable)
        {
            this.GetComponent<HoverToolTip>().useAlternateMessage = false;
        }
        else
        {
            this.GetComponent<HoverToolTip>().useAlternateMessage = true;
        }
    }

    public void unlockUpgrade()
    {
        if (upgraded == false && operationApplied == false && MiscData.dungeonLevelUnlocked >= whatLevelUnlockable)
        {
            if (noLongerUnlockable == false)
            {
                if (prevTile == null)
                {
                    operationApplied = true;
                    if (unlocked == false)
                    {
                        if (PlayerUpgrades.numberSkillPoints >= skillPointsRequirement)
                        {
                            PlayerUpgrades.numberSkillPoints -= skillPointsRequirement;
                            unlocked = true;
                            upgraded = true;
                            lockedIcon.SetActive(false);
                            PlayerUpgrades.musketUpgrades.Add(upgradeID);
                            FindObjectOfType<AudioManager>().PlaySound("Add Upgrade");
                            imageIcon.color = new Color(1, 1, 1, 1);
                        }
                    }
                    else
                    {
                        upgraded = true;
                        PlayerUpgrades.musketUpgrades.Add(upgradeID);
                        FindObjectOfType<AudioManager>().PlaySound("Add Upgrade");
                        imageIcon.color = new Color(1, 1, 1, 1);
                    }
                }
                else
                {
                    if (prevTile.upgraded == true)
                    {
                        operationApplied = true;
                        if (unlocked == false)
                        {
                            if (PlayerUpgrades.numberSkillPoints >= skillPointsRequirement)
                            {
                                PlayerUpgrades.numberSkillPoints -= skillPointsRequirement;
                                unlocked = true;
                                upgraded = true;
                                lockedIcon.SetActive(false);
                                PlayerUpgrades.musketUpgrades.Add(upgradeID);
                                FindObjectOfType<AudioManager>().PlaySound("Add Upgrade");
                                imageIcon.color = new Color(1, 1, 1, 1);

                                if (prevTile.nextTiles.Length > 0)
                                {
                                    if (prevTile.nextTiles.Length > 1)
                                    {
                                        foreach (MusketUpgradeTile tile in prevTile.nextTiles)
                                        {
                                            if (tile != this)
                                            {
                                                tile.noLongerUnlockable = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            upgraded = true;
                            PlayerUpgrades.musketUpgrades.Add(upgradeID);
                            FindObjectOfType<AudioManager>().PlaySound("Add Upgrade");
                            imageIcon.color = new Color(1, 1, 1, 1);
                            if (prevTile.nextTiles.Length > 0)
                            {
                                if (prevTile.nextTiles.Length > 1)
                                {
                                    foreach (MusketUpgradeTile tile in prevTile.nextTiles)
                                    {
                                        if (tile != this)
                                        {
                                            tile.noLongerUnlockable = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (operationApplied == true)
        {
            operationApplied = false;
        }

        SaveSystem.SaveGame();
    }

    bool checkIfUnlocked(MusketUpgradeTile[] tiles)
    {
        foreach(MusketUpgradeTile tile in tiles)
        {
            if(tile.upgraded != false)
            {
                return true;
            }
        }
        return false;
    }

    public void lockUpgrade()
    {/*
        if (upgraded == true && operationApplied == false)
        {
            if (nextTiles.Length > 1)
            {
                if(checkIfUnlocked(nextTiles) == false)
                {
                    operationApplied = true;
                    upgraded = false;
                    PlayerUpgrades.musketUpgrades.Remove(upgradeID);
                    imageIcon.color = new Color(1, 1, 1, 0.63f);

                    if (prevTile.nextTiles.Length > 0)
                    {
                        if (prevTile.nextTiles.Length > 1)
                        {
                            foreach (MusketUpgradeTile tile in prevTile.nextTiles)
                            {
                                if (tile != this)
                                {
                                    tile.noLongerUnlockable = false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (nextTiles.Length == 0)
                {
                    operationApplied = true;
                    upgraded = false;
                    PlayerUpgrades.musketUpgrades.Remove(upgradeID);
                    imageIcon.color = new Color(1, 1, 1, 0.63f);

                    if (prevTile.nextTiles.Length > 0)
                    {
                        if (prevTile.nextTiles.Length > 1)
                        {
                            foreach (MusketUpgradeTile tile in prevTile.nextTiles)
                            {
                                if (tile != this)
                                {
                                    tile.noLongerUnlockable = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (nextTiles[0].upgraded == false)
                    {
                        operationApplied = true;
                        upgraded = false;
                        PlayerUpgrades.musketUpgrades.Remove(upgradeID);
                        imageIcon.color = new Color(1, 1, 1, 0.63f);

                        if (prevTile.nextTiles.Length > 0)
                        {
                            if (prevTile.nextTiles.Length > 1)
                            {
                                foreach (MusketUpgradeTile tile in prevTile.nextTiles)
                                {
                                    if (tile != this)
                                    {
                                        tile.noLongerUnlockable = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (operationApplied == true)
            {
                operationApplied = false;
            }
        }*/
    }
}
