using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletFromAnotherDimension : ArtifactEffect
{
    [SerializeField] GameObject lightningOrb;
    List<GameObject> lightningOrbs = new List<GameObject>();
    int lightningOrbCount = 0;
    float anglePeriod = 0;
    Coroutine mainLoop;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if(lightningOrbs.Count < 5)
        {
            if(lightningOrbCount == 0)
            {
                mainLoop = StartCoroutine(rotateOrbs());
            }

            lightningOrbCount++;
            GameObject lightningOrbInstant = Instantiate(lightningOrb, PlayerProperties.playerShipPosition, Quaternion.identity);
            lightningOrbs.Add(lightningOrbInstant);
        }
    }

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        if (mainLoop != null)
        {
            StopCoroutine(mainLoop);
        }

        lightningOrbCount = 0;
        foreach(GameObject orb in lightningOrbs)
        {
            orb.GetComponent<LightningGauntletOrb>().attackEnemy(enemy);
        }
        lightningOrbs.Clear();
    }

    IEnumerator rotateOrbs()
    {
        while (true)
        {
            if (anglePeriod > Mathf.PI * 2)
            {
                anglePeriod = 0;
            }
            else
            {
                anglePeriod += Time.deltaTime;
            }

            if (lightningOrbCount > 0)
            {
                float angleOffset = 360 / lightningOrbCount;

                for (int i = 0; i < lightningOrbCount; i++)
                {
                    float offset = angleOffset * i * Mathf.Deg2Rad;
                    lightningOrbs[i].transform.position = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(anglePeriod + offset), Mathf.Sin(anglePeriod + offset)) * 2;
                }
            }

            yield return null;
        }
    }
}
