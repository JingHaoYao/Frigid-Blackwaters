using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonChallenge : MonoBehaviour
{
    GameObject spawnedIndicator;
    GameObject playerShip;
    PlayerScript playerScript;
    public GameObject examineIndicator;
    ItemTemplates itemTemplates;
    public int whatTier = 1;
    SpriteRenderer spriteRenderer;
    Animator animator;
    bool activated = false;
    public GameObject aStarGrid;
    AntiSpawnSpaceDetailer anti;
    GameObject spawnedGrid;
    public GameObject chestParticles, summonEffect;
    public Sprite regularCrystal;
    DungeonEntryDialogueManager manager;

    EnemyRoomTemplates enemyTemplates;

    int numberWaves = 2;
    int currRound = 0;
    bool summoningEnemies = false;

    void Start()
    {
        enemyTemplates = FindObjectOfType<EnemyRoomTemplates>();
        playerShip = GameObject.Find("PlayerShip");
        itemTemplates = FindObjectOfType<ItemTemplates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerScript = playerShip.GetComponent<PlayerScript>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        anti = this.transform.parent.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer;
        manager = FindObjectOfType<DungeonEntryDialogueManager>();
    }

    IEnumerator pulse()
    {
        animator.enabled = true;
        animator.SetTrigger("Pulse");
        this.GetComponents<AudioSource>()[0].Play();
        yield return new WaitForSeconds(6f / 12f);
        animator.enabled = false;
        spriteRenderer.sprite = regularCrystal;
    }

    GameObject[] spawnEnemy(int tier, EnemyRoomTemplate template)
    {
        if (manager.whatDungeonLevel == 1)
        {
            if (tier == 1)
            {
                List<GameObject> enemyList = new List<GameObject>();
                foreach (string name in template.potentialEnemyNames)
                {
                    GameObject enemy = Resources.Load<GameObject>("Regular Enemies/First Dungeon Level/Tier 1 Enemies/" + name + "/" + name);
                    enemyList.Add(enemy);
                }
                return enemyList.ToArray();
            }
            else if (tier == 2)
            {
                List<GameObject> enemyList = new List<GameObject>();
                foreach (string name in template.potentialEnemyNames)
                {
                    GameObject enemy = Resources.Load<GameObject>("Regular Enemies/First Dungeon Level/Tier 2 Enemies/" + name + "/" + name);
                    enemyList.Add(enemy);
                }
                return enemyList.ToArray();
            }
            else if (tier == 3)
            {
                List<GameObject> enemyList = new List<GameObject>();
                foreach (string name in template.potentialEnemyNames)
                {
                    GameObject enemy = Resources.Load<GameObject>("Regular Enemies/First Dungeon Level/Tier 3 Enemies/" + name + "/" + name);
                    enemyList.Add(enemy);
                }
                return enemyList.ToArray();
            }
            else
            {
                List<GameObject> enemyList = new List<GameObject>();
                foreach (string name in template.potentialEnemyNames)
                {
                    GameObject enemy = Resources.Load<GameObject>("Regular Enemies/First Dungeon Level/Tier 4 Enemies/" + name + "/" + name);
                    enemyList.Add(enemy);
                }
                return enemyList.ToArray();
            }
        }
        else if(manager.whatDungeonLevel == 2)
        {
            if (tier == 1)
            {
                List<GameObject> enemyList = new List<GameObject>();
                foreach (string name in template.potentialEnemyNames)
                {
                    GameObject enemy = Resources.Load<GameObject>("Regular Enemies/Second Dungeon Level/Tier 1 Enemies/" + name + "/" + name);
                    enemyList.Add(enemy);
                }
                return enemyList.ToArray();
            }
            else if (tier == 2)
            {
                List<GameObject> enemyList = new List<GameObject>();
                foreach (string name in template.potentialEnemyNames)
                {
                    GameObject enemy = Resources.Load<GameObject>("Regular Enemies/Second Dungeon Level/Tier 2 Enemies/" + name + "/" + name);
                    enemyList.Add(enemy);
                }
                return enemyList.ToArray();
            }
            else if (tier == 3)
            {
                List<GameObject> enemyList = new List<GameObject>();
                foreach (string name in template.potentialEnemyNames)
                {
                    GameObject enemy = Resources.Load<GameObject>("Regular Enemies/Second Dungeon Level/Tier 3 Enemies/" + name + "/" + name);
                    enemyList.Add(enemy);
                }
                return enemyList.ToArray();
            }
            else
            {
                List<GameObject> enemyList = new List<GameObject>();
                foreach (string name in template.potentialEnemyNames)
                {
                    GameObject enemy = Resources.Load<GameObject>("Regular Enemies/Second Dungeon Level/Tier 4 Enemies/" + name + "/" + name);
                    enemyList.Add(enemy);
                }
                return enemyList.ToArray();
            }
        }
        else
        {
            return null;
        }
    }

    IEnumerator generateEnemies()
    {
        summoningEnemies = true;
        StartCoroutine(pulse());
        yield return new WaitForSeconds(2f / 12f);
        for (int i = 0; i < 4; i++)
        {
            Vector3 randPos = new Vector3(transform.position.x + Random.Range(-8, 8), transform.position.y + Random.Range(-8, 8), 0);
            while (Physics2D.OverlapCircle(randPos, 0.5f))
            {
                randPos = new Vector3(transform.position.x + Random.Range(-8, 8), transform.position.y + Random.Range(-8, 8), 0);
            }
            GameObject instant = Instantiate(summonEffect, randPos, Quaternion.identity);
            if (whatTier == 1)
            {
                instant.GetComponent<CrystalSummoningEffect>().skeleList = spawnEnemy(1, enemyTemplates.tier1EnemyTemplates[Random.Range(0, enemyTemplates.tier1EnemyTemplates.Length)]);
            }
            else if (whatTier == 2)
            {
                if (Random.Range(0, 3) == 1)
                {
                    instant.GetComponent<CrystalSummoningEffect>().skeleList = spawnEnemy(1, enemyTemplates.tier1EnemyTemplates[Random.Range(0, enemyTemplates.tier1EnemyTemplates.Length)]);
                }
                else
                {
                    instant.GetComponent<CrystalSummoningEffect>().skeleList = spawnEnemy(2, enemyTemplates.tier2EnemyTemplates[Random.Range(0, enemyTemplates.tier2EnemyTemplates.Length)]);
                }
            }
            else
            {
                if (Random.Range(0, 3) == 1)
                {
                    instant.GetComponent<CrystalSummoningEffect>().skeleList = spawnEnemy(2, enemyTemplates.tier2EnemyTemplates[Random.Range(0, enemyTemplates.tier2EnemyTemplates.Length)]);
                }
                else
                {
                    instant.GetComponent<CrystalSummoningEffect>().skeleList = spawnEnemy(3, enemyTemplates.tier3EnemyTemplates[Random.Range(0, enemyTemplates.tier3EnemyTemplates.Length)]);
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
        summoningEnemies = false;
    }


    void LateUpdate()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true && activated == false)
        {
            if (spawnedIndicator == null)
            {
                spawnedIndicator = Instantiate(examineIndicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                spawnedIndicator.GetComponent<ExamineIndicator>().parentObject = this.gameObject;
            }

            if (playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    numberWaves = whatTier + 1;
                    playerScript.enemiesDefeated = false;
                    activated = true;
                    spawnedGrid = Instantiate(aStarGrid, transform.position, Quaternion.identity);
                    animator.enabled = true;
                    anti.trialDefeated = false;
                    anti.spawnDoorSeals();
                }
            }
        }
        else
        {
            if (spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
        }

        if (activated == true && anti.trialDefeated == false && summoningEnemies == false)
        {
            if (EnemyPool.isPoolEmpty())
            {
                if (currRound < numberWaves)
                {
                    currRound++;
                    StartCoroutine(generateEnemies());
                }
                else
                {
                    animator.enabled = true;
                    animator.SetTrigger("Exhaust");
                    this.GetComponents<AudioSource>()[1].Play();
                    GameObject instant = Instantiate(chestParticles, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                    instant.GetComponent<CrystalParticles>().target = transform.position + new Vector3(0, -2f, 0);
                    anti.trialDefeated = true;
                    playerScript.enemiesDefeated = true;
                    GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
                    if (spawnedGrid)
                    {
                        Destroy(spawnedGrid);
                    }
                }
            }
        }
    }
}
