using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassOfZunis : MonoBehaviour
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

    IEnumerator summonVoidExplosion()
    {
        Enemy[] activeEnemies = FindObjectsOfType<Enemy>();
        if (activeEnemies.Length > 0)
        {
            artifacts.numKills -= 4;
            foreach (Enemy enemy in activeEnemies)
            {
                GameObject explosionInstant = Instantiate(voidExplosion, enemy.transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
                explosionInstant.GetComponent<GlassOfZunisExplosion>().targetObject = enemy.gameObject;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 4)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    StartCoroutine(summonVoidExplosion());
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    StartCoroutine(summonVoidExplosion());
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    StartCoroutine(summonVoidExplosion());
                }
            }
        }
    }
}
