using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBombs : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject voidExplosion;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void summonVoidExplosion()
    {
        Enemy[] activeEnemies = FindObjectsOfType<Enemy>();
        if(activeEnemies.Length == 0)
        {
            return;
        }

        artifacts.numKills -= 6;
        float closestDistance = float.MaxValue;
        Enemy targetEnemy = null;
        foreach(Enemy enemy in activeEnemies)
        {
            if(Vector2.Distance(playerScript.transform.position, enemy.transform.position) < closestDistance)
            {
                closestDistance = Vector2.Distance(playerScript.transform.position, enemy.transform.position);
                targetEnemy = enemy;
            }
        }

        Instantiate(voidExplosion, targetEnemy.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 6)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonVoidExplosion();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonVoidExplosion();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonVoidExplosion();
                }
            }
        }
    }
}
