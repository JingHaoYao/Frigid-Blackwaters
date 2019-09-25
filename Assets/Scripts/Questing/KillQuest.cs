using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuest : QuestType
{
    public List<string> enemyFilters;

    public void progressQuest(string id)
    {
        if (enemyFilters.Contains(id))
        {
            currentAmount++;
            Evaluate();
        }
    }
}
