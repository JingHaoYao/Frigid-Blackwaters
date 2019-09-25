using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyRoomTemplate
{
    public string[] potentialEnemyNames;

    public EnemyRoomTemplate(int length)
    {
        potentialEnemyNames = new string[length];
    }
}
