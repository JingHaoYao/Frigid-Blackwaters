using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour {
    public AStarGrid grid;
    public Vector3 seeker, target;
    public List<AStarNode> seekPath = new List<AStarNode>();

    private IEnumerator mainLoop()
    {
        while (true)
        {
            seeker = this.gameObject.transform.position;
            findPath(seeker + new Vector3(0, 0.4f, 0), target);
            yield return null;
        }
    }

    private IEnumerator searchForGrid()
    {
        while (grid == null)
        {
            grid = FindObjectOfType<AStarGrid>();
            yield return null;
        }
        StartCoroutine(mainLoop());
    }

    void Awake () {
        StartCoroutine(searchForGrid());
	}

    void findPath(Vector3 startPos, Vector3 endPos)
    {
        AStarNode startNode = grid.nodeFromWorldPoint(startPos);
        AStarNode endNode = grid.nodeFromWorldPoint(endPos);
        List<AStarNode> openSet = new List<AStarNode>();
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            AStarNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                retracePath(startNode, endNode);
                return;
            }

            foreach (AStarNode neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.traversable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void retracePath(AStarNode _startNode, AStarNode _endNode)
    {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = _endNode;

        while(currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
        seekPath = path;
    }

    int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distanceX > distanceY)
        {
            return 140 * distanceY + 100 * (distanceX - distanceY);
        }
        return 140 * distanceX + 100 * (distanceY - distanceX);
    }
}
