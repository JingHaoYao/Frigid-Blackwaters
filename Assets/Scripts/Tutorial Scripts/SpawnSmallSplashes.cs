using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSmallSplashes : MonoBehaviour
{
    public float splashPeriod = 0;
    public GameObject smallWaterSplash;


    void Update()
    {
        splashPeriod += Time.deltaTime;
        if(splashPeriod > 0.1f)
        {
            splashPeriod = 0;
            for(int i = 0; i < 6; i++)
            {
                Instantiate(smallWaterSplash, Camera.main.transform.position + new Vector3(Random.Range(-9.0f, 9.0f), Random.Range(-9.0f, 9.0f), 0), Quaternion.identity);
            }
        }
    }
}
