using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantLiliaceaeWallWave : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public GameObject waterFoam;
    private float foamTimer = 0;

    void spawnFoam()
    {
        if (spriteRenderer.color.a == 1) {
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position + Vector3.up * 0.75f, Quaternion.Euler(0, 0, 90));
            }
        }
    }

    void Update()
    {
        spawnFoam();
    }
}
