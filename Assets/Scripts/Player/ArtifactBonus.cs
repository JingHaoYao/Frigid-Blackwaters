using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactBonus : MonoBehaviour {
    public int whatDungeonArtifact = 1;
    public float speedBonus;
    public float defenseBonus;
    public int attackBonus;
    public int healthBonus;
    public int periodicHealing;
    public int artifactChanceBonus;
    public int goldBonus;
    public int killRequirement;

    // public parameters that are updated based off the events they are tied to
    // used to signal certain commands and activate artifact effects

    // Whenever the player kills an enemy
    public bool addedKill = false;
    // Whenever the player takes damage
    public bool tookDamage = false;
    // Whenever the player fires the left weapon, and so on
    public bool firedLeftWeapon = false;
    public bool firedFrontWeapon = false;
    public bool firedRightWeapon = false;
    // Whenever the player enters a previously unentered room
    public bool exploredNewRoom = false;
    // Whenever the player picks up an item (updates the inventory)
    public bool updatedInventory = false;
}
