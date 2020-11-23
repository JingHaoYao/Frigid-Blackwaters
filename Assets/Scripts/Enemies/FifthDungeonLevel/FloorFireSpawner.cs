using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFireSpawner : MonoBehaviour
{
    int[,] floorFireInstants = new int[16, 16];
    [SerializeField] GameObject explosion;
    [SerializeField] List<GameObject> floorFires;

    private void Awake()
    {
        EnemyPool.floorFireSpawner = this;
    }

    public void SpawnFloorFires(Vector3 position, float radius)
    {
        Instantiate(explosion, position, Quaternion.identity);

        int radiusDistance = Mathf.FloorToInt(radius);

        int currPositionX = Mathf.RoundToInt(position.x - 0.5f) + 8;
        int currPositionY = Mathf.RoundToInt(position.y - 0.5f) + 8;

        if(currPositionX >= 0 && currPositionY >= 0 && currPositionX < 16 && currPositionY < 16 && !Physics2D.OverlapCircle(transform.position + new Vector3(currPositionX - 7.5f, currPositionY - 7.5f), 0.5f, 12))
        {
            Instantiate(floorFires[Random.Range(0, floorFires.Count)], transform.position + new Vector3(currPositionX - 7.5f, currPositionY - 7.5f), Quaternion.identity);
        }

        int spawnPositionX = currPositionX;
        int spawnPositionY = currPositionY;
        for(int i = 0; i < radiusDistance; i++)
        {
            spawnPositionX++;
            SpawnFloorFire(spawnPositionX, spawnPositionY);
        }

        spawnPositionX = currPositionX;
        spawnPositionY = currPositionY;
        for (int i = 0; i < radiusDistance; i++)
        {
            spawnPositionX--;
            SpawnFloorFire(spawnPositionX, spawnPositionY);
        }

        for (int i = 0; i < radiusDistance; i++)
        {
            spawnPositionX++;
            SpawnFloorFire(spawnPositionX, spawnPositionY);
        }

        spawnPositionX = currPositionX;
        spawnPositionY = currPositionY;
        for (int i = 0; i < radiusDistance; i++)
        {
            spawnPositionY--;
            SpawnFloorFire(spawnPositionX, spawnPositionY);
        }

        spawnPositionX = currPositionX;
        spawnPositionY = currPositionY;
        for (int i = 0; i < radiusDistance; i++)
        {
            spawnPositionY++;
            SpawnFloorFire(spawnPositionX, spawnPositionY);
        }

        spawnPositionX = currPositionX;
        spawnPositionY = currPositionY;
        for (int i = 0; i < radiusDistance - 1; i++)
        {
            spawnPositionY++;
            spawnPositionX++;
            SpawnFloorFire(spawnPositionX, spawnPositionY);
        }

        spawnPositionX = currPositionX;
        spawnPositionY = currPositionY;
        for (int i = 0; i < radiusDistance - 1; i++)
        {
            spawnPositionY--;
            spawnPositionX++;
            SpawnFloorFire(spawnPositionX, spawnPositionY);
        }

        spawnPositionX = currPositionX;
        spawnPositionY = currPositionY;
        for (int i = 0; i < radiusDistance - 1; i++)
        {
            spawnPositionY++;
            spawnPositionX--;
            SpawnFloorFire(spawnPositionX, spawnPositionY);
        }

        spawnPositionX = currPositionX;
        spawnPositionY = currPositionY;
        for (int i = 0; i < radiusDistance - 1; i++)
        {
            spawnPositionY--;
            spawnPositionX--;
            SpawnFloorFire(spawnPositionX, spawnPositionY);
        }

    }

    void SpawnFloorFire(int spawnX, int spawnY)
    {
        if (spawnY >= 0 && spawnY < 16 && spawnX >= 0 && spawnX < 16 && floorFireInstants[spawnX, spawnY] == 0 && !Physics2D.OverlapCircle(transform.position + new Vector3(spawnX - 7.5f, spawnY - 7.5f, 12), 0.5f, 12))
        {
            Instantiate(floorFires[Random.Range(0, floorFires.Count)], transform.position + new Vector3(spawnX - 7.5f, spawnY - 7.5f), Quaternion.identity);
        }
    }

    public void AddFloorFire(int xIndex, int yIndex)
    {
        floorFireInstants[xIndex, yIndex] = 1;
    }

    public void RemoveFloorFire(int xIndex, int yIndex)
    {
        floorFireInstants[xIndex, yIndex] = 0;
    }
}
