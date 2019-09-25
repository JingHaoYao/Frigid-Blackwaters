using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreQuest : QuestType
{
    public void progressQuest(int numberRoomsVisited)
    {
        currentAmount = numberRoomsVisited;
        Evaluate();
    }
}
