using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomLayout {
    public Vector3[] largeObstaclePositions;
    public Vector3[] smallObstaclePositions;

    //
    // Short obstacles are those that players/mobs cannot
    // move over but they can shoot over.
    //
    public Vector3[] largeShortObstaclePositions;
    public Vector3[] smallShortObstaclePositions;

    RoomLayout()
    {
        largeObstaclePositions = new Vector3[0];
        smallObstaclePositions = new Vector3[0];
    }

    public RoomLayout(
        Vector3[] _largeObstaclePositions = null, 
        Vector3[] _smallObstaclePositions = null, 
        Vector3[] _largeShortObstaclePositions = null, 
        Vector3[] _smallShortObstaclePositions = null
    )
    {
        largeObstaclePositions = _largeObstaclePositions;
        smallObstaclePositions = _smallObstaclePositions;
        largeShortObstaclePositions = _largeShortObstaclePositions;
        smallShortObstaclePositions = _smallShortObstaclePositions;
    }

    public Vector3[] getLargeObstaclePositions()
    {
        return largeObstaclePositions;
    }

    public Vector3[] getSmallObstaclePositions()
    {
        return smallObstaclePositions;
    }

    public Vector3[] getLargeShortObstaclePositions()
    {
        return largeShortObstaclePositions;
    }

    public Vector3[] getSmallShortObstaclePositions()
    {
        return smallShortObstaclePositions;
    }
}
