using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObstacleTemplates : MonoBehaviour {
    public string[] theme1SpecialObstacleNames;
    public string[] theme2SpecialObstacleNames;

    /*private void Start()
    {
        theme1SpecialObstacleNames = new string[theme1SpecialObstacles.Length];
        theme2SpecialObstacleNames = new string[theme2SpecialObstacles.Length];

        for(int i = 0; i < theme1SpecialObstacleNames.Length; i++)
        {
            theme1SpecialObstacleNames[i] = theme1SpecialObstacles[i].name;
        }

        for (int i = 0; i < theme2SpecialObstacleNames.Length; i++)
        {
            theme2SpecialObstacleNames[i] = theme2SpecialObstacles[i].name;
        }
    }*/

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
        else
        {
            return null;
        }
    }
}
