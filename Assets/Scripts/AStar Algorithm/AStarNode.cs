using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode {
    public bool traversable;
    public Vector3 nodePosition;
    public int gCost;
    public int hCost;
    public int gridX, gridY;
    public AStarNode parent;

    public AStarNode(bool _traversable, Vector3 _nodePosition, int _gridX, int _gridY)
    {
        traversable = _traversable;
        nodePosition = _nodePosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
