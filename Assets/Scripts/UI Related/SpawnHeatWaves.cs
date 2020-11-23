using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHeatWaves : MonoBehaviour
{
    [SerializeField] GameObject heatWave;

    List<GameObject> heatWaveInstances = new List<GameObject>();

    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnHeatWaveRoutine());
    }

    IEnumerator SpawnHeatWaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
            SpawnHeatWave();
        }
    }

    Vector3 PickSpotToSpawnHeatWave()
    {
        float bound = mainCamera.orthographicSize * 1.25f;

        return new Vector3(mainCamera.transform.position.x + Random.Range(0, 2) == 1 ? bound : -bound, mainCamera.transform.position.y + Random.Range(0, 2) == 1 ? bound : -bound);
    }

    void SpawnHeatWave()
    {
        foreach(GameObject heatWaveInstant in heatWaveInstances)
        {
            if (heatWaveInstant.activeSelf == false)
            {
                heatWaveInstant.transform.position = PickSpotToSpawnHeatWave();
                heatWaveInstant.SetActive(true);
                return;
            }
        }

        GameObject newInstant = Instantiate(heatWave, PickSpotToSpawnHeatWave(), Quaternion.identity);
        heatWaveInstances.Add(newInstant);
    }
}
