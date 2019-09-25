using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineQuest : QuestType
{
    public List<GameObject> previouslyExaminedObstacles = new List<GameObject>();

    public void progressQuest(GameObject examinedObject)
    {
        if (!previouslyExaminedObstacles.Contains(examinedObject))
        {
            previouslyExaminedObstacles.Add(examinedObject);
            currentAmount++;
        }
        Evaluate();
    }
}
