using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiSpawnSpaceDetailer : MonoBehaviour {
    BoxCollider2D boxCol;
    public bool leftOpening, rightOpening, topOpening, bottomOpening;
    public GameObject waterTile, roomReveal, doorwaySeal;
    bool hasSpawnedWaterTile = false;
    Camera mainCamera;
    private bool roomInit = false;
    private bool roomDone = false;
    private bool spawningComplete = false;
    GameObject[] doorSeals;
    private int dangerValueSum = 0, dangerValueCap = 0, numberEnemiesSummoned = 0;
    GameObject playerShip;
    SolidObstaclesTemplates solidObstacleTemplates;
    ObstacleLayoutTemplates obstacleLayouts;
    public GameObject gridMap;
    private GameObject spawnedGridMap;
    private bool emptyRoom = false;
    public GameObject treasureChest;
    SpecialObstacleTemplates specialObstacleTemplates;
    SpecialRoomTemplates specialRoomTemplates;
    bool specialRoom = false;
    public bool trialDefeated = false;
    public bool checkPointRoom = false;
    public bool spawnRoom = false;
    public MapUI mapUI;
    EnemyRoomTemplates enemyRoomTemplates;
    DungeonEntryDialogueManager dialogueManager;
    PlayerScript playerScript;
    [SerializeField] private RoomInteraction uniqueInteraction;

    //
    // What room type (used for mapping)
    // 1 - spawn room
    // 2 - regular combat room/empty room (no icon)
    // 3 - examinable object in the room
    // 4 - treasure rooms
    // 5 - mini boss rooms
    // 6 - shop rooms
    // 7 - checkpoint room
    // 8 - trove rooms
    // 9 - gamble rooms
    // 10 - wave challenge rooms
    // 11 - altar rooms
    // 
    public int whatRoomType = 1;

    void setDangerValueCap()
    {
        dangerValueCap = Mathf.RoundToInt((Vector3.Magnitude(transform.parent.transform.position) / 20)) + Mathf.RoundToInt(playerScript.numRoomsVisited / 3f);
    }

    bool isPositionPicked(List<Vector3> posArray, Vector3 pos)
    {
        for (int i = 0; i < posArray.Count; i++)
        {
            if (Vector2.Distance(posArray[i], pos) < 1 || Vector2.Distance(pos, playerShip.transform.position) < 7 || Physics2D.OverlapCircle(pos, 0.5f))
            {
                return true;
            }
        }
        return false;
    }

    void spawnChest()
    {
        int spawnChestRate = 70 + dangerValueCap * 3;
        if(Random.Range(1,101) < spawnChestRate)
        {
            Vector3 spawnPos = new Vector3(Random.Range(mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8), Random.Range(mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8), 0);
            while(Physics2D.OverlapCircle(spawnPos, 0.5f) == true)
            {
                spawnPos = new Vector3(Random.Range(mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8), Random.Range(mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8), 0);
            }
            Instantiate(treasureChest, spawnPos, Quaternion.identity);
        }
    }

    void spawnObstacles()
    {
        GameObject spawnedObstacle;
        int whatLayout = Random.Range(0, obstacleLayouts.roomLayouts.Count);
        SetRoomDesign design = transform.parent.GetComponent<SetRoomDesign>();

        int whatTheme = 2;
        if (design.theme2Designs.Contains(design.whichDesign))
        {
            whatTheme = 1;
        }
        if (Random.Range(0, 8) == 0)
        {
            emptyRoom = true;
        }
        else
        {
            if (obstacleLayouts.roomLayouts[whatLayout].getLargeObstaclePositions().Length != 0)
            {
                for (int i = 0; i < obstacleLayouts.roomLayouts[whatLayout].getLargeObstaclePositions().Length; i++)
                {
                    if (whatTheme == 1)
                    {
                        spawnedObstacle = Instantiate(solidObstacleTemplates.solidObstaclesTemplatesBigTheme2[Random.Range(0, solidObstacleTemplates.solidObstaclesTemplatesBigTheme2.Length)], obstacleLayouts.roomLayouts[whatLayout].getLargeObstaclePositions()[i] + mainCamera.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        spawnedObstacle = Instantiate(solidObstacleTemplates.solidObstaclesTemplatesBigTheme1[Random.Range(0, solidObstacleTemplates.solidObstaclesTemplatesBigTheme1.Length)], obstacleLayouts.roomLayouts[whatLayout].getLargeObstaclePositions()[i] + mainCamera.transform.position, Quaternion.identity);
                    }

                    spawnedObstacle.transform.SetParent(this.transform.parent);

                    if (Random.Range(0, 2) == 1)
                    {
                        Vector3 localScale = spawnedObstacle.transform.localScale;
                        localScale.x *= -1;
                        spawnedObstacle.transform.localScale = localScale;
                    }
                    uniqueInteraction?.AddObstacle(spawnedObstacle);
                }
            }

            if (obstacleLayouts.roomLayouts[whatLayout].getSmallObstaclePositions() != null && obstacleLayouts.roomLayouts[whatLayout].getSmallObstaclePositions().Length != 0)
            {
                for (int i = 0; i < obstacleLayouts.roomLayouts[whatLayout].getSmallObstaclePositions().Length; i++)
                {
                    if (whatTheme == 1)
                    {
                        spawnedObstacle = Instantiate(solidObstacleTemplates.solidObstaclesTemplatesSmallTheme2[Random.Range(0, solidObstacleTemplates.solidObstaclesTemplatesSmallTheme2.Length)], obstacleLayouts.roomLayouts[whatLayout].getSmallObstaclePositions()[i] + mainCamera.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        spawnedObstacle = Instantiate(solidObstacleTemplates.solidObstaclesTemplatesSmallTheme1[Random.Range(0, solidObstacleTemplates.solidObstaclesTemplatesSmallTheme1.Length)], obstacleLayouts.roomLayouts[whatLayout].getSmallObstaclePositions()[i] + mainCamera.transform.position, Quaternion.identity);
                    }

                    spawnedObstacle.transform.SetParent(this.transform.parent);

                    if (Random.Range(0, 2) == 1)
                    {
                        Vector3 localScale = spawnedObstacle.transform.localScale;
                        localScale.x *= -1;
                        spawnedObstacle.transform.localScale = localScale;
                    }
                    uniqueInteraction?.AddObstacle(spawnedObstacle);
                }
            }

            if (obstacleLayouts.roomLayouts[whatLayout].getSmallShortObstaclePositions() != null && obstacleLayouts.roomLayouts[whatLayout].getSmallShortObstaclePositions().Length != 0)
            {
                for (int i = 0; i < obstacleLayouts.roomLayouts[whatLayout].getSmallShortObstaclePositions().Length; i++)
                {
                    spawnedObstacle = Instantiate(solidObstacleTemplates.solidObstacleTemplatesSmallShort[Random.Range(0, solidObstacleTemplates.solidObstacleTemplatesSmallShort.Length)], obstacleLayouts.roomLayouts[whatLayout].getSmallShortObstaclePositions()[i] + mainCamera.transform.position, Quaternion.identity);
                    if (Random.Range(0, 2) == 1)
                    {
                        Vector3 localScale = spawnedObstacle.transform.localScale;
                        localScale.x *= -1;
                        spawnedObstacle.transform.localScale = localScale;
                    }

                    spawnedObstacle.transform.SetParent(this.transform.parent);
                    uniqueInteraction?.AddObstacle(spawnedObstacle);
                }
            }

            if (obstacleLayouts.roomLayouts[whatLayout].getLargeShortObstaclePositions() != null && obstacleLayouts.roomLayouts[whatLayout].getLargeShortObstaclePositions().Length != 0)
            {
                for (int i = 0; i < obstacleLayouts.roomLayouts[whatLayout].getLargeShortObstaclePositions().Length; i++)
                {
                    spawnedObstacle = Instantiate(solidObstacleTemplates.solidObstacleTemplatesLargeShort[Random.Range(0, solidObstacleTemplates.solidObstacleTemplatesLargeShort.Length)], obstacleLayouts.roomLayouts[whatLayout].getLargeShortObstaclePositions()[i] + mainCamera.transform.position, Quaternion.identity);
                    if (Random.Range(0, 2) == 1)
                    {
                        Vector3 localScale = spawnedObstacle.transform.localScale;
                        localScale.x *= -1;
                        spawnedObstacle.transform.localScale = localScale;
                    }

                    spawnedObstacle.transform.SetParent(this.transform.parent);

                    uniqueInteraction?.AddObstacle(spawnedObstacle);
                }
            }

            if (obstacleLayouts.roomLayouts[whatLayout].getSmallObstaclePositions().Length == 0 && obstacleLayouts.roomLayouts[whatLayout].getLargeObstaclePositions().Length == 0)
            {
                emptyRoom = true;
            }
        }

        if (enemyRoomTemplates.emptyRoomEnemyNames.Length > 0)
        {
            GameObject emptyRoomEnemy = pickEmptyNonTemplateEnemy(false);

            if (emptyRoom == true && Random.Range(1, 101) <= emptyRoomEnemy.GetComponent<Enemy>().percentSpawnChance)
            {
                Vector3 spawnPosition = pickRandEnemySpawn();
                while (Physics2D.OverlapCircle(spawnPosition, 0.5f))
                {
                    spawnPosition = pickRandEnemySpawn();
                }
                Instantiate(emptyRoomEnemy, spawnPosition, Quaternion.identity);
                dangerValueSum += emptyRoomEnemy.GetComponent<Enemy>().dangerValue;
                numberEnemiesSummoned += emptyRoomEnemy.GetComponent<Enemy>().dangerValue;
            }
        }
        spawnedGridMap = Instantiate(gridMap, mainCamera.transform.position, Quaternion.identity);
    }

    void spawnSpecialObstacles()
    {
        SetRoomDesign design = transform.parent.GetComponent<SetRoomDesign>();

        int whatTheme = 1;
        if (design.theme2Designs.Contains(design.whichDesign))
        {
            whatTheme = 2;
        }
        GameObject specialObstacle = Instantiate(specialObstacleTemplates.loadRandomSpecialObstacle(dialogueManager.whatDungeonLevel, whatTheme), transform.position, Quaternion.identity);
        specialObstacle.transform.SetParent(this.transform.parent);

        uniqueInteraction?.AddObstacle(specialObstacle);
    }

    Vector3 pickRandEnemySpawn()
    {
        float xSpawn = Mathf.RoundToInt(Random.Range(mainCamera.transform.position.x - 7, mainCamera.transform.position.x + 7)) - 0.5f + Random.Range(0,2);
        float ySpawn = Mathf.RoundToInt(Random.Range(mainCamera.transform.position.y - 7, mainCamera.transform.position.y + 7)) - 0.5f + Random.Range(0,2);
        Vector3 test = new Vector3(xSpawn, ySpawn, 0);
        while(Physics2D.OverlapCircle(test, 0.5f) == true)
        {
            xSpawn = Mathf.RoundToInt(Random.Range(mainCamera.transform.position.x - 7, mainCamera.transform.position.x + 7)) - 0.5f + Random.Range(0, 2);
            ySpawn = Mathf.RoundToInt(Random.Range(mainCamera.transform.position.y - 7, mainCamera.transform.position.y + 7)) - 0.5f + Random.Range(0, 2);
            test = new Vector3(xSpawn, ySpawn, 0);
        }
        return test;
    }

    // Commented out code for spawning interactable debris, may want to revisit in the future, but for the time being
    // debris just clutters room and has too much visual feedback when the player should focus on enemies
    /*void spawnDebris()
    {
        int numberDebris = Random.Range(4, 8);

        for(int i = 0; i < numberDebris; i++)
        {
            Vector3 spawnPosition = pickRandEnemySpawn();

            GameObject debrisInstant = Instantiate(solidObstacleTemplates.debrisObjects[Random.Range(0, solidObstacleTemplates.debrisObjects.Length)], spawnPosition, Quaternion.identity);
            debrisInstant.transform.SetParent(transform.parent);
            if(Random.Range(0, 2) == 1)
            {
                Vector3 scale = debrisInstant.transform.localScale;
                debrisInstant.transform.localScale = new Vector3(-1 * scale.x, scale.y);
            }
        }
    }*/

    int spawnEnemy(int tier, EnemyRoomTemplate template, Vector3 spawnPos)
    {
        string dungeonName = "";
        switch (dialogueManager.whatDungeonLevel)
        {
            case 1:
                dungeonName = "First Dungeon Level";
                break;
            case 2:
                dungeonName = "Second Dungeon Level";
                break;
            case 3:
                dungeonName = "Third Dungeon Level";
                break;
            case 4:
                dungeonName = "Fourth Dungeon Level";
                break;
            case 5:
                dungeonName = "Fifth Dungeon Level";
                break;
        }

        if (tier == 1)
        {
            int index = Random.Range(0, template.potentialEnemyNames.Length);
            GameObject enemy = null;
            for (int i = 1; i < tier + 1; i++)
            {
                enemy = Resources.Load<GameObject>("Regular Enemies/" + dungeonName + "/Tier " + i.ToString() + " Enemies/" + template.potentialEnemyNames[index] + "/" + template.potentialEnemyNames[index]);
                if (enemy != null)
                {
                    GameObject spawnedEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
                    Enemy enemyClass = spawnedEnemy.GetComponent<Enemy>();
                    EnemyPool.addEnemy(enemyClass);
                    return enemyClass.dangerValue;
                }
            }


            if (enemy == null)
            {
                Debug.LogError(enemy + " " + template.potentialEnemyNames[index]);
            }

            return 0;
        }
        else if (tier == 2)
        {
            int index = Random.Range(0, template.potentialEnemyNames.Length);
            GameObject enemy = null;
            for (int i = 1; i < tier + 1; i++)
            {
                enemy = Resources.Load<GameObject>("Regular Enemies/" + dungeonName + "/Tier " + i.ToString() + " Enemies/" + template.potentialEnemyNames[index] + "/" + template.potentialEnemyNames[index]);
                if(enemy != null)
                {
                    GameObject spawnedEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
                    Enemy enemyClass = spawnedEnemy.GetComponent<Enemy>();
                    EnemyPool.addEnemy(enemyClass);
                    return enemyClass.dangerValue;
                }
            }

            if(enemy == null)
            {
                Debug.LogError(enemy + " " + template.potentialEnemyNames[index]);
            }

            return 0;
        }
        else if (tier == 3)
        {
            int index = Random.Range(0, template.potentialEnemyNames.Length);
            GameObject enemy = null;
            for (int i = 1; i < tier + 1; i++)
            {
                enemy = Resources.Load<GameObject>("Regular Enemies/" + dungeonName + "/Tier " + i.ToString() + " Enemies/" + template.potentialEnemyNames[index] + "/" + template.potentialEnemyNames[index]);
                if (enemy != null)
                {
                    GameObject spawnedEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
                    Enemy enemyClass = spawnedEnemy.GetComponent<Enemy>();
                    EnemyPool.addEnemy(enemyClass);
                    return enemyClass.dangerValue;
                }
            }


            if (enemy == null)
            {
                Debug.LogError(enemy + " " + template.potentialEnemyNames[index]);
            }

            return 0;
        }
        else if (tier == 4)
        {
            int index = Random.Range(0, template.potentialEnemyNames.Length);
            GameObject enemy = null;
            for (int i = 1; i < tier + 1; i++)
            {
                enemy = Resources.Load<GameObject>("Regular Enemies/" + dungeonName + "/Tier " + i.ToString() + " Enemies/" + template.potentialEnemyNames[index] + "/" + template.potentialEnemyNames[index]);
                if (enemy != null)
                {
                    GameObject spawnedEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
                    Enemy enemyClass = spawnedEnemy.GetComponent<Enemy>();
                    EnemyPool.addEnemy(enemyClass);
                    return enemyClass.dangerValue;
                }
            }

            if (enemy == null)
            {
                Debug.LogError(enemy + " " + template.potentialEnemyNames[index]);
            }

            return 0;
        }
        else
        {
            return 0;
        }
    }

    GameObject pickEmptyNonTemplateEnemy(bool templateEnemy)
    {
        string dungeonName = "";
        switch (dialogueManager.whatDungeonLevel)
        {
            case 1:
                dungeonName = "First Dungeon Level";
                break;
            case 2:
                dungeonName = "Second Dungeon Level";
                break;
            case 3:
                dungeonName = "Third Dungeon Level";
                break;
            case 4:
                dungeonName = "Fourth Dungeon Level";
                break;
            case 5:
                dungeonName = "Fifth Dungeon Level";
                break;
        }

        if (templateEnemy == true)
        {
            string enemy = enemyRoomTemplates.nonRoomTemplateEnemyNames[Random.Range(0, enemyRoomTemplates.nonRoomTemplateEnemyNames.Length)];
            GameObject pickedEnemy = Resources.Load<GameObject>("Regular Enemies/" + dungeonName + "/Non Template Enemies/" + enemy + "/" + enemy);
            return pickedEnemy;
        }
        else
        {
            string enemy = enemyRoomTemplates.emptyRoomEnemyNames[Random.Range(0, enemyRoomTemplates.emptyRoomEnemyNames.Length)];
            GameObject emptyRoomEnemy = Resources.Load<GameObject>("Regular Enemies/" + dungeonName + "/Empty Room Enemies/" + enemy + "/" + enemy);
            return emptyRoomEnemy;
        }
    }

    int spawnLimitEnemies(EnemyRoomTemplate template, Vector3 spawnPos)
    {
        if(template == null)
        {
            return 0;
        }

        if (Random.Range(0, 2) == 1 && template.enemiesWithLimits.Length > 0)
        {
            string dungeonName = "";
            switch (dialogueManager.whatDungeonLevel)
            {
                case 1:
                    dungeonName = "First Dungeon Level";
                    break;
                case 2:
                    dungeonName = "Second Dungeon Level";
                    break;
                case 3:
                    dungeonName = "Third Dungeon Level";
                    break;
                case 4:
                    dungeonName = "Fourth Dungeon Level";
                    break;
                case 5:
                    dungeonName = "Fifth Dungeon Level";
                    break;
            }

            int index = Random.Range(0, template.enemiesWithLimits.Length);
            GameObject enemy = null;
            for (int i = 1; i < 5; i++)
            {
                enemy = Resources.Load<GameObject>("Regular Enemies/" + dungeonName + "/Tier " + i.ToString() + " Enemies/" + template.enemiesWithLimits[index] + "/" + template.enemiesWithLimits[index]);
                if (enemy != null)
                {
                    GameObject spawnedEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
                    Enemy enemyClass = spawnedEnemy.GetComponent<Enemy>();
                    EnemyPool.addEnemy(enemyClass);
                    return enemyClass.dangerValue;
                }
            }
        }

        return 0;
    }

    public void EndRoomSpawn()
    {
        foreach(RoomSpawn roomSpawner in transform.parent.GetComponentsInChildren<RoomSpawn>())
        {
            roomSpawner.EndRoomSpawn();
        }

        transform.parent.GetComponent<SetRoomDesign>().Initialize();
    }

    void spawnEnemies()
    {
        List<Vector3> posArray = new List<Vector3>();
        Vector3 spawnPosition;
        GameObject spawnedEnemy;

        if(dangerValueCap >= 0 && dangerValueCap < 2)
        {
            //spawn a tier 1 enemy room
            EnemyRoomTemplate template = enemyRoomTemplates.tier1EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier1EnemyTemplates.Length)];
            while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
            {
                spawnPosition = pickRandEnemySpawn();
                while (isPositionPicked(posArray, spawnPosition) == true)
                {
                    spawnPosition = pickRandEnemySpawn();
                }

                numberEnemiesSummoned++;
                dangerValueSum += spawnEnemy(1, template, spawnPosition);
                posArray.Add(spawnPosition);
            }

            spawnPosition = pickRandEnemySpawn();
            spawnLimitEnemies(template, spawnPosition);
        }
        else if(dangerValueCap >= 2 && dangerValueCap < 4)
        {
            if (Random.Range(0, 3) == 1) //33% chance to spawn a tier 2 enemy room
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier2EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier2EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(2, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);
            }
            else
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier1EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier1EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(1, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);
            }
        }
        else if(dangerValueCap >= 4 && dangerValueCap < 6)
        {
            int typeRoom = Random.Range(0, 4);
            if (typeRoom == 1) //25% chance to spawn a tier 3 enemy room
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier3EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier3EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(3, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);

                //spawn non template enemy
                if (enemyRoomTemplates.nonRoomTemplateEnemyNames.Length > 0)
                {
                    GameObject pickedEnemy = pickEmptyNonTemplateEnemy(true);

                    if (Random.Range(1, 101) <= pickedEnemy.GetComponent<Enemy>().percentSpawnChance)
                    {
                        spawnPosition = pickRandEnemySpawn();
                        while (isPositionPicked(posArray, spawnPosition) == true)
                        {
                            spawnPosition = pickRandEnemySpawn();
                        }

                        numberEnemiesSummoned++;
                        spawnedEnemy = Instantiate(pickedEnemy, spawnPosition, Quaternion.identity);
                        Enemy enemyClass = spawnedEnemy.GetComponent<Enemy>();
                        dangerValueSum += enemyClass.dangerValue;
                        EnemyPool.addEnemy(enemyClass);
                        posArray.Add(spawnPosition);
                    }
                }
            }
            else if (typeRoom == 2)
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier2EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier2EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(2, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);
            }
            else
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier1EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier1EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(1, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);
            }
        }
        else if(dangerValueCap >= 6 && dangerValueCap < 8)
        {
            int typeRoom = Random.Range(0, 6);
            if (typeRoom < 3)
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier3EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier3EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(3, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);

                if (enemyRoomTemplates.nonRoomTemplateEnemyNames.Length > 0)
                {
                    GameObject pickedEnemy = pickEmptyNonTemplateEnemy(true);

                    if (Random.Range(1, 101) <= pickedEnemy.GetComponent<Enemy>().percentSpawnChance)
                    {
                        spawnPosition = pickRandEnemySpawn();
                        while (isPositionPicked(posArray, spawnPosition) == true)
                        {
                            spawnPosition = pickRandEnemySpawn();
                        }

                        numberEnemiesSummoned++;
                        spawnedEnemy = Instantiate(pickedEnemy, spawnPosition, Quaternion.identity);
                        Enemy enemyClass = spawnedEnemy.GetComponent<Enemy>();
                        dangerValueSum += enemyClass.dangerValue;
                        EnemyPool.addEnemy(enemyClass);
                        posArray.Add(spawnPosition);
                    }

                }
            }
            else if (typeRoom >= 3 && typeRoom < 5)
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier2EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier2EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(2, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);
            }
            else
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier1EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier1EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(1, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);
            }
         
        }
        else if(dangerValueCap >= 8 && dangerValueCap < 10)
        {
            int typeRoom = Random.Range(0, 8);
            if (typeRoom < 2) //50% chance to spawn a tier 4 enemy
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier4EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier4EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 6)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(4, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);

                if (enemyRoomTemplates.nonRoomTemplateEnemyNames.Length > 0)
                {
                    GameObject pickedEnemy = pickEmptyNonTemplateEnemy(true);

                    if (Random.Range(1, 101) <= pickedEnemy.GetComponent<Enemy>().percentSpawnChance)
                    {
                        spawnPosition = pickRandEnemySpawn();
                        while (isPositionPicked(posArray, spawnPosition) == true)
                        {
                            spawnPosition = pickRandEnemySpawn();
                        }

                        numberEnemiesSummoned++;
                        spawnedEnemy = Instantiate(pickedEnemy, spawnPosition, Quaternion.identity);
                        Enemy enemyClass = spawnedEnemy.GetComponent<Enemy>();
                        dangerValueSum += enemyClass.dangerValue;
                        EnemyPool.addEnemy(enemyClass);
                        posArray.Add(spawnPosition);
                    }
                }
            }
            else if (typeRoom >= 2 && typeRoom < 5)
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier3EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier3EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(3, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);
            }
            else
            {
                EnemyRoomTemplate template = enemyRoomTemplates.tier2EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier2EnemyTemplates.Length)];
                while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 4)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    dangerValueSum += spawnEnemy(2, template, spawnPosition);
                    posArray.Add(spawnPosition);
                }

                spawnPosition = pickRandEnemySpawn();
                spawnLimitEnemies(template, spawnPosition);
            }
        }
        else
        {
            EnemyRoomTemplate template = enemyRoomTemplates.tier4EnemyTemplates[Random.Range(0, enemyRoomTemplates.tier4EnemyTemplates.Length)];
            while (dangerValueSum <= dangerValueCap && numberEnemiesSummoned < 6)
            {
                spawnPosition = pickRandEnemySpawn();
                while (isPositionPicked(posArray, spawnPosition) == true)
                {
                    spawnPosition = pickRandEnemySpawn();
                }

                numberEnemiesSummoned++;
                dangerValueSum += spawnEnemy(4, template, spawnPosition);
                posArray.Add(spawnPosition);
            }

            spawnPosition = pickRandEnemySpawn();
            spawnLimitEnemies(template, spawnPosition);

            if (enemyRoomTemplates.nonRoomTemplateEnemyNames.Length > 0)
            {
                GameObject pickedEnemy = pickEmptyNonTemplateEnemy(true);

                if (Random.Range(1, 101) <= pickedEnemy.GetComponent<Enemy>().percentSpawnChance)
                {
                    spawnPosition = pickRandEnemySpawn();
                    while (isPositionPicked(posArray, spawnPosition) == true)
                    {
                        spawnPosition = pickRandEnemySpawn();
                    }

                    numberEnemiesSummoned++;
                    spawnedEnemy = Instantiate(pickedEnemy, spawnPosition, Quaternion.identity);
                    Enemy enemyClass = spawnedEnemy.GetComponent<Enemy>();
                    dangerValueSum += enemyClass.dangerValue;
                    EnemyPool.addEnemy(enemyClass);
                    posArray.Add(spawnPosition);
                }
            }
        }

        spawnChest();
        spawningComplete = true;
    }


    IEnumerator adjustPlayer()
    {
        //moves the player forward by a bit to adjust for ice walls spawning
        if (playerShip.transform.position.y > transform.parent.transform.position.y + 5)
        {
            playerShip.transform.position += new Vector3(0, -2f, 0);
        }
        else if (playerShip.transform.position.x > transform.parent.transform.position.x + 5)
        {
            playerShip.transform.position += new Vector3(-2f, 0, 0);
        }
        else if (playerShip.transform.position.x < transform.parent.transform.position.x - 5)
        {
            playerShip.transform.position += new Vector3(2f, 0, 0);
        }
        else
        {
            playerShip.transform.position += new Vector3(0, 2f, 0);
        }
        playerShip.GetComponent<PlayerScript>().addRootingObject();
        yield return new WaitForSeconds(0.2f);
        playerShip.GetComponent<PlayerScript>().removeRootingObject();
    }

    void openDoorSeals()
    {
        for(int i = 0; i < 4; i++)
        {
            if (doorSeals[i] != null)
            {
                doorSeals[i].GetComponent<DoorSeal>().open = true;
            }
        }
        if (spawnedGridMap)
        {
            Destroy(spawnedGridMap);
        }
    }

    public void spawnDoorSeals()
    {
        if(leftOpening)
        {
            doorSeals[0] = Instantiate(doorwaySeal, transform.parent.transform.position + new Vector3(-10.5f, 0, 0), Quaternion.identity);
            doorSeals[0].transform.SetParent(this.transform.parent);
        }
        if (rightOpening)
        {
            doorSeals[1] = Instantiate(doorwaySeal, transform.parent.transform.position + new Vector3(10.5f, 0, 0), Quaternion.Euler(0, 0, 180));
            doorSeals[1].transform.SetParent(this.transform.parent);
        }
        if (topOpening)
        {
            doorSeals[2] = Instantiate(doorwaySeal, transform.parent.transform.position + new Vector3(0, 10.5f, 0), Quaternion.Euler(0, 0, 270));
            doorSeals[2].transform.SetParent(this.transform.parent);
        }
        if (bottomOpening)
        {
            doorSeals[3] = Instantiate(doorwaySeal, transform.parent.transform.position + new Vector3(0, -10.5f, 0), Quaternion.Euler(0, 0, 90));
            doorSeals[3].transform.SetParent(this.transform.parent);
        }
    }

    void setRoomMemory()
    {
        if (!leftOpening)
        {
            transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[3] = 1;
        }
        if (!rightOpening)
        {
            transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[2] = 1;
        }
        if (!topOpening)
        {
            transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[0] = 1;
        }
        if (!bottomOpening)
        {
            transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[1] = 1;
        }
    }

    private void Start()
    {
        setRoomMemory();
        setRoomType();
        adjustTo20();
        mainCamera = Camera.main;
        enemyRoomTemplates = FindObjectOfType<EnemyRoomTemplates>();
        dialogueManager = FindObjectOfType<DungeonEntryDialogueManager>();
        boxCol = GetComponent<BoxCollider2D>();
        solidObstacleTemplates = GameObject.Find("SolidObstacleTemplates").GetComponent<SolidObstaclesTemplates>();
        obstacleLayouts = GameObject.Find("ObstacleLayoutTemplates").GetComponent<ObstacleLayoutTemplates>();
        specialObstacleTemplates = GameObject.Find("SpecialObstacleTemplates").GetComponent<SpecialObstacleTemplates>();
        specialRoomTemplates = GameObject.Find("SpecialRoomTemplates").GetComponent<SpecialRoomTemplates>();
        playerShip = PlayerProperties.playerShip;
        playerScript = playerShip.GetComponent<PlayerScript>();
        doorSeals = new GameObject[4];
        GameObject waterTileInstant = Instantiate(waterTile, transform.position, Quaternion.identity);
        waterTileInstant.transform.SetParent(this.transform.parent);
        GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().antiList.Add(this);
        mapUI = playerShip.GetComponent<MapUI>();
    }

    void adjustTo20()
    {
        int xPos = Mathf.RoundToInt(transform.parent.position.x / 20f);
        int yPos = Mathf.RoundToInt(transform.parent.position.y / 20f);
        transform.parent.position = new Vector2(xPos * 20, yPos * 20);
    }

    //
    // What room type (used for mapping)
    // 1 - spawn room
    // 2 - regular combat room/empty room (no icon)
    // 3 - examinable object in the room
    // 4 - treasure rooms - 10%
    // 5 - mini boss rooms - 20%
    // 6 - shop rooms = - 15%
    // 7 - checkpoint room
    // 8 - trove rooms - 10%
    // 9 - gamble rooms - 15%
    // 10 - wave challenge rooms - 15%
    // 11 - altar rooms - 15%
    // 
    public void setRoomType()
    {
        if (transform.parent.gameObject.name != "SpawnRoom" && transform.parent.gameObject.name != "SpawnRoom(Clone)")
        {
            int whatRoomGen = Random.Range(1, 101);
            if (checkPointRoom == false)
            {
                if (whatRoomGen < 65)
                {
                    whatRoomType = 2;
                }
                else if (whatRoomGen >= 65 && whatRoomGen < 80)
                {
                    whatRoomType = 3;
                }
                else
                {
                    int whatUniqueRoom = Random.Range(1, 101);
                    if (whatUniqueRoom < 20)
                    {
                        whatRoomType = 5;
                    }
                    else if (whatUniqueRoom >= 20 && whatUniqueRoom < 35)
                    {
                        whatRoomType = 6;
                    }
                    else if (whatUniqueRoom >= 35 && whatUniqueRoom < 45)
                    {
                        whatRoomType = 4;
                    }
                    else if (whatUniqueRoom >= 45 && whatUniqueRoom < 55)
                    {
                        whatRoomType = 8;
                    }
                    else if (whatUniqueRoom >= 55 && whatUniqueRoom < 70)
                    {
                        whatRoomType = 9;
                    }
                    else if (whatUniqueRoom >= 70 && whatUniqueRoom < 85)
                    {
                        whatRoomType = 10;
                    }
                    else
                    {
                        whatRoomType = 11;
                    }
                }
            }
            else
            {
                whatRoomType = 7;
            }
        }
        else
        {
            whatRoomType = 1;
        }

        transform.parent.GetComponent<SetRoomDesign>().setRoomID(whatRoomType);
    }

    int whatTier(int dangerValueCap)
    {
        if(dangerValueCap < 4)
        {
            return 1;
        }
        else if(dangerValueCap >= 4 && dangerValueCap < 7)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    void triggerDialogue(int whatType)
    {
        DialogueUI dialogueUI = dialogueManager.dialogueUI;
        DialogueSet targetDialogue = null;

        switch (whatType){
            case 4:
                if (!MiscData.completedUniqueRoomsDialogues.Contains("Treasure Dialogue"))
                    targetDialogue = Resources.Load<DialogueSet>("Dialogues/Unique Room Dialogue/Treasure Dialogue");
                break;
            case 5:
                if (!MiscData.completedUniqueRoomsDialogues.Contains("Combat Dialogue"))
                    targetDialogue = Resources.Load<DialogueSet>("Dialogues/Unique Room Dialogue/Combat Dialogue");
                break;
            case 6:
                if (!MiscData.completedUniqueRoomsDialogues.Contains("Shop Dialogue"))
                    targetDialogue = Resources.Load<DialogueSet>("Dialogues/Unique Room Dialogue/Shop Dialogue");
                break;
            case 8:
                if (!MiscData.completedUniqueRoomsDialogues.Contains("Trove Dialogue"))
                    targetDialogue = Resources.Load<DialogueSet>("Dialogues/Unique Room Dialogue/Trove Dialogue");
                break;
            case 9:
                if (!MiscData.completedUniqueRoomsDialogues.Contains("Gamble Dialogue"))
                    targetDialogue = Resources.Load<DialogueSet>("Dialogues/Unique Room Dialogue/Gamble Dialogue");
                break;
            case 10:
                if (!MiscData.completedUniqueRoomsDialogues.Contains("Trial Dialogue"))
                    targetDialogue = Resources.Load<DialogueSet>("Dialogues/Unique Room Dialogue/Trial Dialogue");
                break;
            case 11:
                if (!MiscData.completedUniqueRoomsDialogues.Contains("Altar Dialogue"))
                    targetDialogue = Resources.Load<DialogueSet>("Dialogues/Unique Room Dialogue/Altar Dialogue");
                break;
        }

        if (targetDialogue != null)
        {
            dialogueUI.LoadDialogueUI(targetDialogue, 0.1f);
            playerScript.playerDead = true;
        }
    }

    public void Initialize()
    {
        if (hasSpawnedWaterTile == false)
        {
            boxCol.enabled = false;
            hasSpawnedWaterTile = true;
            mapUI.mapLoaded = false;
            mapUI.loadingMap = true;
        }
    }

    private void LateUpdate()
    {
        if (checkPointRoom == false && roomDone == false)
        {
            if (transform.parent.gameObject.name != "SpawnRoom")
            {
                if (Vector2.Distance(mainCamera.transform.position, transform.parent.position) < 0.5f)
                {
                    if (specialRoom == false)
                    {
                        if (spawningComplete == true && roomDone == false && EnemyPool.isPoolEmpty())
                        {
                            openDoorSeals();
                            roomDone = true;
                            playerScript.enemiesDefeated = true;
                            uniqueInteraction?.RoomFinished();
                        }
                    }
                    else
                    {
                        if (trialDefeated == true && roomDone == false)
                        {
                            openDoorSeals();
                            roomDone = true;
                            playerScript.enemiesDefeated = true;
                            uniqueInteraction?.RoomFinished();
                        }
                    }
                }
            }
        }
    }

    public void InitializeRoomEntry()
    {
        if(roomInit == true)
        {
            return;
        }

        if (checkPointRoom == false)
        {
            setDangerValueCap();
            playerScript.numRoomsSinceLastArtifact++;
            playerScript.numRoomsVisited++;
            Instantiate(roomReveal, transform.position, Quaternion.identity);

            if (playerScript.numRoomsVisited == 5)
            {
                MiscData.enoughRoomsTraversed = true;
                SaveSystem.SaveGame();
            }

            foreach (ArtifactSlot slot in PlayerProperties.playerArtifacts.artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().exploredNewRoom(whatRoomType);
            }

            if(PlayerProperties.flammableController != null)
            {
                PlayerProperties.flammableController.RemoveFlammableStack();
            }

            if (whatRoomType == 2)
            {
                spawnDoorSeals();
                spawnObstacles();
                //spawnDebris();
                spawnEnemies();
                playerScript.enemiesDefeated = false;
            }
            else if (whatRoomType == 3)
            {
                spawnSpecialObstacles();
            }
            else if (whatRoomType == 4)
            {
                GameObject instant = Instantiate(specialRoomTemplates.loadUniqueRoom(whatTier(dangerValueCap), 6), transform.position, Quaternion.identity);
                instant.transform.SetParent(this.transform.parent);
                specialRoom = true;
                if (instant.GetComponent<WhichRoomManager>())
                {
                    instant.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = this;
                }
            }
            else if (whatRoomType == 5)
            {
                GameObject instant = Instantiate(specialRoomTemplates.loadUniqueRoom(whatTier(dangerValueCap), 4), transform.position, Quaternion.identity);
                specialRoom = true;
                if (instant.GetComponent<WhichRoomManager>())
                {
                    instant.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = this;
                }
            }
            else if (whatRoomType == 6)
            {
                GameObject instant = Instantiate(specialRoomTemplates.loadUniqueRoom(whatTier(dangerValueCap), 1), transform.position, Quaternion.identity);
                instant.transform.SetParent(this.transform.parent);
                specialRoom = true;
                if (instant.GetComponent<WhichRoomManager>())
                {
                    instant.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = this;
                }
            }
            else if (whatRoomType == 8)
            {
                GameObject instant = Instantiate(specialRoomTemplates.loadUniqueRoom(whatTier(dangerValueCap), 5), transform.position, Quaternion.identity);
                instant.transform.SetParent(this.transform.parent);
                specialRoom = true;
                if (instant.GetComponent<WhichRoomManager>())
                {
                    instant.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = this;
                }
            }
            else if (whatRoomType == 9)
            {
                GameObject instant = Instantiate(specialRoomTemplates.loadUniqueRoom(whatTier(dangerValueCap), 2), transform.position, Quaternion.identity);
                instant.transform.SetParent(this.transform.parent);
                specialRoom = true;
                if (instant.GetComponent<WhichRoomManager>())
                {
                    instant.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = this;
                }
            }
            else if (whatRoomType == 10)
            {
                GameObject instant = Instantiate(specialRoomTemplates.loadUniqueRoom(whatTier(dangerValueCap), 3), transform.position, Quaternion.identity);
                instant.transform.SetParent(this.transform.parent);
                specialRoom = true;
                if (instant.GetComponent<WhichRoomManager>())
                {
                    instant.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = this;
                }
            }
            else if (whatRoomType == 11)
            {
                GameObject instant = Instantiate(specialRoomTemplates.loadUniqueRoom(whatTier(dangerValueCap), 7), transform.position, Quaternion.identity);
                instant.transform.SetParent(this.transform.parent);
                specialRoom = true;
                if (instant.GetComponent<WhichRoomManager>())
                {
                    instant.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = this;
                }
            }
            triggerDialogue(whatRoomType);
            StartCoroutine(adjustPlayer());
            roomInit = true;
            playerScript.periodicHeal();
            uniqueInteraction?.RoomInitialized(dangerValueCap);
        }
        else
        {
            roomInit = true;

            int whichSide = 0;

            if (bottomOpening)
            {
                whichSide = 4;
            }
            else if (topOpening)
            {
                whichSide = 2;
            }
            else if (rightOpening)
            {
                whichSide = 1;
            }
            else
            {
                whichSide = 3;
            }

            FindObjectOfType<MissionManager>().activateBossManager(whichSide);

            if (PlayerProperties.flammableController != null)
            {
                PlayerProperties.flammableController.RemoveFlammableStack();
            }

            playerScript.periodicHeal();
            playerScript.numRoomsSinceLastArtifact++;
            playerScript.numRoomsVisited++;
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "AntiSpawnSpaceSpawner" && transform.parent.name != "SpawnRoom") {
            //If two rooms overlap by random instancing, destroy both
            GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().antiList.Remove(this);
            Destroy(transform.parent.gameObject);
        }
    }
}
