using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObstacleTemplates : MonoBehaviour {
    public string[] theme1SpecialObstacleNames;
    public string[] theme2SpecialObstacleNames;

    public GameObject loadRandomSpecialObstacle(int whatDungeonLevel, int whatTheme = 1)
    {
        if(whatDungeonLevel == 1)
        {
            if (whatTheme == 1)
            {
                return Resources.Load<GameObject>("Unique Obstacles/First Dungeon Level/" + theme1SpecialObstacleNames[Random.Range(0, theme1SpecialObstacleNames.Length)]);
            }
            else
            {
                return Resources.Load<GameObject>("Unique Obstacles/First Dungeon Level/" + theme2SpecialObstacleNames[Random.Range(0, theme2SpecialObstacleNames.Length)]);
            }
        }
        else if(whatDungeonLevel == 2)
        {
            return Resources.Load<GameObject>("Unique Obstacles/Second Dungeon Level/" + theme1SpecialObstacleNames[Random.Range(0, theme1SpecialObstacleNames.Length)]);
        }
        else if(whatDungeonLevel == 3)
        {
            return Resources.Load<GameObject>("Unique Obstacles/Third Dungeon Level/" + theme1SpecialObstacleNames[Random.Range(0, theme1SpecialObstacleNames.Length)]);
        }
        else if(whatDungeonLevel == 4)
        {
            return Resources.Load<GameObject>("Unique Obstacles/Fourth Dungeon Level/" + theme1SpecialObstacleNames[Random.Range(0, theme1SpecialObstacleNames.Length)]);
        }
        else
        {
            return null;
        }
    }
}
