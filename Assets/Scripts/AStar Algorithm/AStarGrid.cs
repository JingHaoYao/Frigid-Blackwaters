using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour {
    public LayerMask unWalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    AStarNode[,] grid;
    GameObject playerShip;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    public bool showGizmos = false;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        playerShip = PlayerProperties.playerShip;
    }

    public List<AStarNode> GetNeighbours(AStarNode node)
    {
        List<AStarNode> neighbours = new List<AStarNode>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((y == 0 && x == 0) /*|| (y + x == 0) || (Mathf.Abs(y + x) == 2)*/)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    void CreateGrid()
    {
        grid = new AStarNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius + 0.2f, unWalkableMask));
                grid[x, y] = new AStarNode(walkable, worldPoint, x, y);
            }
        }
    }

    public AStarNode nodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x - transform.position.x + (gridWorldSize.x / 2)) / (gridWorldSize.x);
        float percentY = (worldPos.y - transform.position.y + (gridWorldSize.y / 2)) / (gridWorldSize.y);
        percentX = Mathf.Clamp(percentX, 0, 1);
        percentY = Mathf.Clamp(percentY, 0, 1);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<AStarNode> path;

    private void OnDrawGizmos()
    {
        if (showGizmos) {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));

            if (grid != null)
            {
                AStarNode playerNode = nodeFromWorldPoint(playerShip.transform.position);
                foreach (AStarNode node in grid)
                {
                    Gizmos.color = (node.traversable) ? Color.white : Color.red;
                    if (playerNode == node)
                    {
                        Gizmos.color = Color.blue;
                    }

                    /*if (path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }*/
                    Gizmos.DrawCube(node.nodePosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }
}
