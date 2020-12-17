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
    }
}
