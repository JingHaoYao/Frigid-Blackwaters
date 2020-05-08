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

    private float fogCycleInBetweenDuration;
    private float fogCycleDuration;

    float fogCyclePeriod = 0;

    FourthLevelFogController fogController;

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
        Debug.Log(fogCycleDuration);
        StartCoroutine(fogCycle());
    }

    IEnumerator fogCycle()
    {
        while (EnemyPool.enemyPool.Count > 0)
        {
            while(fogCyclePeriod < fogCycleInBetweenDuration)
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
            glowRunes();

            while(fogCyclePeriod < fogCycleDuration)
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
    }

    private void unGlowRunes()
    {
        LeanTween.value(1, 0, 0.75f).setOnUpdate((float val) => runesSpriteRenderer.color = new Color(r, g, b, val));
    }
}
