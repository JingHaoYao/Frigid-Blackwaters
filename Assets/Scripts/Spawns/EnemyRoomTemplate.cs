using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyRoomTemplate
{
    public string[] potentialEnemyNames;
    public string[] enemiesWithLimits;

    public EnemyRoomTemplate(int length, int enemiesWithLimitsLength = 0)
    {
        potentialEnemyNames = new string[length];
        enemiesWithLimits = new string[enemiesWithLimitsLength];
    }
}
