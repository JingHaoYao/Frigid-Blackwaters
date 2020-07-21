using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarField : ArtifactEffect
{
    [SerializeField] GameObject moon;
    GameObject moonInstant;
    Animator moonInstantAnimator;
    [SerializeField] GameObject wave1, wave2, wave3, wave4, wave5;
    [SerializeField] AudioSource audio;
    Coroutine mainLoopInstant;
    Coroutine mainAttackLoopInstant;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public override void artifactEquipped()
    {
        mainLoopInstant = StartCoroutine(mainLoop());
        mainAttackLoopInstant = StartCoroutine(mainAttackLoop());
    }

    public override void artifactUnequipped()
    {
        moonInstantAnimator.SetTrigger("Explode");
        Destroy(moonInstant, 0.5f);
        StopCoroutine(mainLoopInstant);
        StopCoroutine(mainAttackLoopInstant);
    }

    void summonWaves(int angle)
    {
        Vector3 spawnPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 5;

        if (Mathf.Abs(mainCamera.transform.position.x - spawnPosition.x) > 8f || Mathf.Abs(mainCamera.transform.position.y - spawnPosition.y) > 8f)
        {
            return;
        }

        GameObject instant;
        switch (angle)
        {
            case 90:
                Instantiate(wave1, spawnPosition, Quaternion.identity);
                break;
            case 45:
                Instantiate(wave2, spawnPosition, Quaternion.identity);
                break;
            case 135:
                instant = Instantiate(wave2, spawnPosition, Quaternion.identity);
                instant.transform.localScale = new Vector3(-4, 4);
                break;
            case 180:
                instant = Instantiate(wave3, spawnPosition, Quaternion.identity);
                instant.transform.localScale = new Vector3(-4, 4);
                break;
            case 225:
                instant = Instantiate(wave4, spawnPosition, Quaternion.identity);
                instant.transform.localScale = new Vector3(-4, 4);
                break;
            case 270:
                instant = Instantiate(wave5, spawnPosition, Quaternion.identity);
                break;
            case 315:
                instant = Instantiate(wave4, spawnPosition, Quaternion.identity);
                break;
            default:
                instant = Instantiate(wave3, spawnPosition, Quaternion.identity);
                break;
        }
    }

    IEnumerator mainAttackLoop()
    {
        yield return new WaitForSeconds(5 / 12f);

        while (true)
        {
            audio.Play();
            moonInstantAnimator.SetTrigger("Pull");
            for(int i = 0; i < 8; i++)
            {
                int angle = i * 45;
                summonWaves(angle);
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator mainLoop()
    {
        moonInstant = Instantiate(moon, PlayerProperties.playerShipPosition, Quaternion.identity);
        SpriteRenderer moonRenderer = moonInstant.GetComponent<SpriteRenderer>();
        moonInstantAnimator = moonInstant.GetComponent<Animator>();
        while (true)
        {
            moonInstant.transform.position = PlayerProperties.playerShipPosition + (Vector3.up * 3);
            moonRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder;
            yield return null;
        }
    }


}
