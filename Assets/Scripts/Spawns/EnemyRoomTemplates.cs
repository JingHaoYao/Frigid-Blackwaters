using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyRoomTemplates : MonoBehaviour
{
    public EnemyRoomTemplate[] tier1EnemyTemplates;
    public EnemyRoomTemplate[] tier2EnemyTemplates;
    public EnemyRoomTemplate[] tier3EnemyTemplates;
    public EnemyRoomTemplate[] tier4EnemyTemplates;
    public string[] emptyRoomEnemyNames;
    public string[] nonRoomTemplateEnemyNames;


    /*private void Start()
    {
        tier1EnemyTemplates = new EnemyRoomTemplate[tier1EnemyRoomTemplates.Length];
        tier2EnemyTemplates = new EnemyRoomTemplate[tier2EnemyRoomTemplates.Length];
        tier3EnemyTemplates = new EnemyRoomTemplate[tier3EnemyRoomTemplates.Length];
        tier4EnemyTemplates = new EnemyRoomTemplate[tier4EnemyRoomTemplates.Length];

        for (int i = 0; i < tier1EnemyRoomTemplates.Length; i++)
        {
            tier1EnemyTemplates[i] = new EnemyRoomTemplate(tier1EnemyRoomTemplates[i].potentialEnemyList.Length);
            int count = tier1EnemyTemplates[i].potentialEnemyNames.Length;
            for (int k = 0; k < count; k++)
            {
                tier1EnemyTemplates[i].potentialEnemyNames[k] = tier1EnemyRoomTemplates[i].potentialEnemyList[k].name;
            }
        }

        for (int i = 0; i < tier2EnemyRoomTemplates.Length; i++)
        {
            tier2EnemyTemplates[i] = new EnemyRoomTemplate(tier2EnemyRoomTemplates[i].potentialEnemyList.Length);
            for (int k = 0; k < tier2EnemyTemplates[i].potentialEnemyNames.Length; k++)
            {
                tier2EnemyTemplates[i].potentialEnemyNames[k] = tier2EnemyRoomTemplates[i].potentialEnemyList[k].name;
            }
        }

        for (int i = 0; i < tier3EnemyRoomTemplates.Length; i++)
        {
            tier3EnemyTemplates[i] = new EnemyRoomTemplate(tier3EnemyRoomTemplates[i].potentialEnemyList.Length);
            for (int k = 0; k < tier3EnemyTemplates[i].potentialEnemyNames.Length; k++)
            {
                tier3EnemyTemplates[i].potentialEnemyNames[k] = tier3EnemyRoomTemplates[i].potentialEnemyList[k].name;
            }
        }

        for (int i = 0; i < tier4EnemyRoomTemplates.Length; i++)
        {
            tier4EnemyTemplates[i] = new EnemyRoomTemplate(tier4EnemyRoomTemplates[i].potentialEnemyList.Length);
            for (int k = 0; k < tier4EnemyTemplates[i].potentialEnemyNames.Length; k++)
            {
                tier4EnemyTemplates[i].potentialEnemyNames[k] = tier4EnemyRoomTemplates[i].potentialEnemyList[k].name;
            }
        }
    }*/
}

