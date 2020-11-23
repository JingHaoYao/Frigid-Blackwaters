using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCycleRoom : RoomInteraction
{
    [SerializeField] Sprite[] runeSprites;
    public GameObject runes;
    public SetRoomDesign roomDesign;
    SpriteRenderer runesSpriteRenderer;
    private const float r = 0, g = 0.9171f, b = 1;

    private List<FogRuneObstacle> fogRuneObstacles = new List<FogRuneObstacle>();

    private float fogCycleInBetweenDuration;
    private float fogCycleDuration;

    float fogCyclePeriod = 0;

    FourthLevelFogController fogController;

    [SerializeField] GameObject invisibilityStatusEffect;

    public void SetFogController(FourthLevelFogController fogController)
    {
        this.fogController = fogController;
    }

    private void Start()
    {
        runesSpriteRenderer = runes.GetComponent<SpriteRenderer>();
        runesSpriteRenderer.sprite = runeSprites[roomDesign.whichDesign];
        runesSpriteRenderer.color = new Color(r, g, b, 0);

    }

    public override void RoomInitialized(int dangerValue)
    {
        fogCycleInBetweenDuration = Mathf.Clamp((10 - dangerValue), 2, float.MaxValue);
        fogCycleDuration = Mathf.Clamp(3 + (dangerValue - 1), 1.5f, 8f);
        StartCoroutine(fogCycle());

        foreach (GameObject spawnedObstacle in allSpawnedObstacles)
        {
            fogRuneObstacles.Add(spawnedObstacle.GetComponent<FogRuneObstacle>());
        }
    }

    IEnumerator fogCycle()
    {
        while (EnemyPool.enemyPool.Count > 0)
        {
            float fogDurationToReduce = 0;

            foreach(Enemy enemy in EnemyPool.enemyPool)
            {
                fogDurationToReduce += enemy.fogStats.fogCoolDownDecrease;
            }

            while(fogCyclePeriod < fogCycleInBetweenDuration - fogDurationToReduce)
            {
                fogCyclePeriod += Time.deltaTime;

                if(EnemyPool.enemyPool.Count == 0)
                {
                    break;
                }
                yield return null;
            }

            fogCyclePeriod = 0;
            fogController.ActivateFog();

            float totalDurationToAdd = 0;
            foreach(Enemy enemy in EnemyPool.enemyPool)
            {
                totalDurationToAdd += enemy.fogStats.fogDurationIncrease;
            }
            float totalDuration = fogCycleDuration + totalDurationToAdd;

            applyInvisStatusEffects(totalDuration);
            glowRunes();

            while(fogCyclePeriod < totalDuration)
            {
                fogCyclePeriod += Time.deltaTime;
                if(EnemyPool.enemyPool.Count == 0)
                {
                    break;
                }
                yield return null;
            }

            fogController.DeActivateFog();
            unGlowRunes();

            fogCyclePeriod = 0;

            yield return null;
        }
        LeanTween.cancelAll(this.gameObject);
        fogController.DeActivateFog();
        unGlowRunes();
    }

    private void glowRunes()
    {
        LeanTween.value(0, 1, 0.75f).setOnUpdate((float val) => runesSpriteRenderer.color = new Color(r, g, b, val));

        foreach(FogRuneObstacle obstacle in fogRuneObstacles)
        {
            obstacle.glowRunes();
        }
    }

    private void unGlowRunes()
    {
        LeanTween.value(1, 0, 0.75f).setOnUpdate((float val) => runesSpriteRenderer.color = new Color(r, g, b, val));

        foreach (FogRuneObstacle obstacle in fogRuneObstacles)
        {
            obstacle.unGlowRunes();
        }
    }

    public void applyInvisStatusEffects(float duration)
    {
        foreach (Enemy enemy in EnemyPool.enemyPool)
        {
            if (enemy != null)
            {
                GameObject effectInstant = Instantiate(invisibilityStatusEffect, enemy.transform.position, Quaternion.identity);
                enemy.addStatus(effectInstant.GetComponent<InvisibilityStatusEffect>(), duration);
            }
        }
    }
}
