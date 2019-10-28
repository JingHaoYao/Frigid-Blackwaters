using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactActiveStatBoost : MonoBehaviour
{
    ArtifactBonus artifactBonus;
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public int healAmount = 0;
    public float speedAmount = 0;
    public float defenseAmount = 0;
    public int attackAmount = 0;
    public float duration = 0;
    bool appliedBonuses = false;
    public GameObject spawnObject;
    public bool stickToShip = false;
    GameObject instant;

    IEnumerator activateArtifact(float duration)
    {
        playerScript.activeEnabled = true;
        if (spawnObject != null)
        {
            instant = Instantiate(spawnObject, playerScript.gameObject.transform.position, Quaternion.identity);
        }
        playerScript.healPlayer(healAmount);
        playerScript.conAttackBonus += attackAmount;
        playerScript.conDefenseBonus += defenseAmount;
        playerScript.conSpeedBonus += speedAmount;
        appliedBonuses = true;
        if (duration > 0)
        {
            FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, duration);
        }
        yield return new WaitForSeconds(duration);
        playerScript.conAttackBonus -= attackAmount;
        playerScript.conDefenseBonus -= defenseAmount;
        playerScript.conSpeedBonus -= speedAmount;
        appliedBonuses = false;
        playerScript.activeEnabled = false;
    }

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifactBonus = GetComponent<ArtifactBonus>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && playerScript.activeEnabled == false && artifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    artifacts.numKills -= artifactBonus.killRequirement;
                    FindObjectOfType<AudioManager>().PlaySound("Generic Artifact Sound");
                    StartCoroutine(activateArtifact(duration));
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    artifacts.numKills -= artifactBonus.killRequirement;
                    FindObjectOfType<AudioManager>().PlaySound("Generic Artifact Sound");
                    StartCoroutine(activateArtifact(duration));
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    artifacts.numKills -= artifactBonus.killRequirement;
                    FindObjectOfType<AudioManager>().PlaySound("Generic Artifact Sound");
                    StartCoroutine(activateArtifact(duration));
                }
            }
        }

        if(instant != null && stickToShip == true)
        {
            instant.transform.position = playerScript.gameObject.transform.position;
        }
    }

    private void OnDestroy()
    {
        if(appliedBonuses == true)
        {
            playerScript.conAttackBonus -= attackAmount;
            playerScript.conDefenseBonus -= defenseAmount;
            playerScript.conSpeedBonus -= speedAmount;
        }
    }
}
