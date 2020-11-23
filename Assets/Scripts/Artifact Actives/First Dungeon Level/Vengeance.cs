using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vengeance : MonoBehaviour {
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject annihilationFigure, redTint;
    public List<GameObject> figureList = new List<GameObject>();
    bool vengeanceEnabled = false;
    GameObject spawnedTint;

    IEnumerator spawnFigures(GameObject[] ActiveRangedEnemies, GameObject[] ActiveMeleeEnemies, GameObject[] ActiveShieldEnemies, GameObject[] ActiveStrongEnemies)
    {
        for (int i = 0; i < ActiveRangedEnemies.Length; i++)
        {
            GameObject instant = Instantiate(annihilationFigure, ActiveRangedEnemies[i].transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
            instant.GetComponent<AnnihilationFigure>().targetDestroy = ActiveRangedEnemies[i];
            figureList.Add(instant);
            instant.GetComponent<AnnihilationFigure>().vengeance = this;
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < ActiveMeleeEnemies.Length; i++)
        {
            GameObject instant = Instantiate(annihilationFigure, ActiveMeleeEnemies[i].transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
            instant.GetComponent<AnnihilationFigure>().targetDestroy = ActiveMeleeEnemies[i];
            figureList.Add(instant);
            instant.GetComponent<AnnihilationFigure>().vengeance = this;
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < ActiveShieldEnemies.Length; i++)
        {
            GameObject instant = Instantiate(annihilationFigure, ActiveShieldEnemies[i].transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
            instant.GetComponent<AnnihilationFigure>().targetDestroy = ActiveShieldEnemies[i];
            figureList.Add(instant);
            instant.GetComponent<AnnihilationFigure>().vengeance = this;
            yield return new WaitForSeconds(0.1f);
        }
        for(int i = 0; i < ActiveStrongEnemies.Length; i++)
        {
            GameObject instant = Instantiate(annihilationFigure, ActiveStrongEnemies[i].transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
            instant.GetComponent<AnnihilationFigure>().targetDestroy = ActiveStrongEnemies[i];
            figureList.Add(instant);
            instant.GetComponent<AnnihilationFigure>().vengeance = this;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void annilihation()
    {
        vengeanceEnabled = true;
        Camera.main.gameObject.GetComponent<CameraShake>().shakeCamFunction(2, 0.1f);
        FindObjectOfType<AudioManager>().PlaySound("Vengeance Eerie Sound");
        playerScript.shipHealth = Mathf.RoundToInt(playerScript.shipHealth / 2);
        GameObject[] ActiveRangedEnemies = GameObject.FindGameObjectsWithTag("RangedEnemy");
        GameObject[] ActiveMeleeEnemies = GameObject.FindGameObjectsWithTag("MeleeEnemy");
        GameObject[] ActiveShieldEnemies = GameObject.FindGameObjectsWithTag("EnemyShield");
        GameObject[] ActiveStrongEnemies = GameObject.FindGameObjectsWithTag("StrongEnemy");
        spawnedTint = Instantiate(redTint, Camera.main.transform.position, Quaternion.identity);
        for (int i = 0; i < ActiveRangedEnemies.Length; i++)
        {
            ActiveRangedEnemies[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        for (int i = 0; i < ActiveMeleeEnemies.Length; i++)
        {
            ActiveMeleeEnemies[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        for(int i = 0; i < ActiveShieldEnemies.Length; i++)
        {
            ActiveShieldEnemies[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        foreach(GameObject enemy in ActiveStrongEnemies)
        {
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        StartCoroutine(spawnFigures(ActiveRangedEnemies, ActiveMeleeEnemies, ActiveShieldEnemies, ActiveStrongEnemies));
    }

    void Start () {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

	void Update () {
        if (displayItem.isEquipped == true && vengeanceEnabled == false &&  artifacts.numKills >= 20 && (GameObject.FindGameObjectWithTag("RangedEnemy") || GameObject.FindGameObjectWithTag("MeleeEnemy") || GameObject.FindGameObjectWithTag("EnemyShield")))
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    annilihation();
                    artifacts.numKills -= 20;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    annilihation();
                    artifacts.numKills -= 20;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    annilihation();
                    artifacts.numKills -= 20;
                }
            }
        }

        if(figureList.Count == 0 && vengeanceEnabled == true)
        {
            spawnedTint.GetComponent<Animator>().SetTrigger("FadeOut");
            Destroy(spawnedTint, 0.833f);
            vengeanceEnabled = false;
        }
    }
}
