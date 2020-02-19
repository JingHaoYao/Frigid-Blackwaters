using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInteraction : MonoBehaviour
{
    public List<GameObject> allSpawnedObstacles = new List<GameObject>();

    public virtual void RoomInitialized(int dangerValue)
    {

    }

    public virtual void RoomFinished()
    {

    }

    public virtual void AddObstacle(GameObject obstacle)
    {
        allSpawnedObstacles.Add(obstacle);
    }
}
