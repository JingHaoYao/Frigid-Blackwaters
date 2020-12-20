using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactBonus : MonoBehaviour{
    public string artifactName;

    public int whatDungeonArtifact = 1;
    public float speedBonus;
    public float defenseBonus;
    public int attackBonus;
    public int healthBonus;
    public int periodicHealing;
    public int artifactChanceBonus;
    public int goldBonus;
    public int killRequirement;
    public int whatRarity;

    public Text descriptionText;
    public Text effectText;
}
