using System.Collections;
using UnityEngine;

public class SniperStunEffect : EnemyStatusEffect
{
    public GameObject[] stars;
    float rotationTimer = 0;

    public override void durationFinishedProcedure()
    {
        StopAllCoroutines();
        foreach(GameObject star in stars)
        {
            LeanTween.alpha(star, 0, 0.5f);
        }
        targetEnemy.removeStatus(this);
        Destroy(this.gameObject, 0.5f);
    }

    IEnumerator spriteRenderAdjustment()
    {
        while (true)
        {
            transform.position = targetEnemy.transform.position + Vector3.up;
            adjustStarsPosition();
            yield return null;
        }
    }

    private void Start()
    {
        StartCoroutine(spriteRenderAdjustment());
        StartCoroutine(waitForSeconds(duration));
    }

    IEnumerator waitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        durationFinishedProcedure();
    }

    void adjustStarsPosition()
    {
        rotationTimer += Time.deltaTime * 4;
        for(int i = 0; i < 3; i++)
        {
            float angle = ((2 * Mathf.PI) / 3) * i;
            stars[i].transform.position = transform.position + new Vector3(Mathf.Cos(rotationTimer + angle) * 1.5f, Mathf.Sin(rotationTimer + angle)) * 0.5f;
        }
    }

}
