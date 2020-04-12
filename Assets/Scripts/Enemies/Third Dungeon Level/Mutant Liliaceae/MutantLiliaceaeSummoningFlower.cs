using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantLiliaceaeSummoningFlower : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToSpawn;
    [SerializeField] AudioSource summoningAudio;
    public MutantLiliaceae boss;

    private void Start()
    {
        StartCoroutine(startingProcedure());
    }

    IEnumerator startingProcedure()
    {
        yield return new WaitForSeconds(6 / 12f);
        GameObject podJumperEnemyInstant = Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], transform.position, Quaternion.identity);
        summoningAudio.Play();
        boss.addJumper(podJumperEnemyInstant);
        yield return new WaitForSeconds(13 / 12f);
        Destroy(this.gameObject);
    }
}
