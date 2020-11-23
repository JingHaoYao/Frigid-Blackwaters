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
    public List<string> filteredEnemyNamesForDungeonTrials;
}

