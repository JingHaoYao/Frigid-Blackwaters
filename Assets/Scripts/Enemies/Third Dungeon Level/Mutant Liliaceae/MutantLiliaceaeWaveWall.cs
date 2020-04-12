using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantLiliaceaeWaveWall : MonoBehaviour
{
    [SerializeField] GameObject waveWall;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] Vector3 endingPosition;
    [SerializeField] float moveTime = 15;
    Vector3 previousuSpawnPosition;

    private void Start()
    {
        transform.position = startingPosition;
        previousuSpawnPosition = transform.position;
        Instantiate(waveWall, transform.position, Quaternion.identity);
        StartCoroutine(spawnWaveWalls());
        LeanTween.move(this.gameObject, endingPosition, moveTime).setOnComplete(() => Destroy(this.gameObject));
    }

    IEnumerator spawnWaveWalls()
    {
        while (true)
        {
            if(Mathf.Abs(transform.position.y - previousuSpawnPosition.y) > 2.5f)
            {
                Instantiate(waveWall, transform.position, Quaternion.identity);
                previousuSpawnPosition = transform.position;
            }

            yield return null;
        }
    }
}
