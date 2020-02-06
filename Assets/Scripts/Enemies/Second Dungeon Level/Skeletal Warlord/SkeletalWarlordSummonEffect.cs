using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWarlordSummonEffect : MonoBehaviour
{
    public GameObject[] skeletonList;
    public GameObject armourEffect;
    SpriteRenderer spriteRenderer;
    GameObject playerShip;
    public SkeletalWarlord skeletalWarlordScript;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    IEnumerator spawnSkele()
    {
        yield return new WaitForSeconds(9 / 12f);
        GameObject spawnedEnemy = Instantiate(skeletonList[Random.Range(0, skeletonList.Length)], transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
        spawnedEnemy.GetComponent<Enemy>().armorMitigation = 1;
        EnemyPool.addEnemy(spawnedEnemy.GetComponent<Enemy>());
        for (int i = 0; i < skeletalWarlordScript.spawnedEnemies.Length; i++)
        {
            if(skeletalWarlordScript.spawnedEnemies[i] == null)
            {
                skeletalWarlordScript.spawnedEnemies[i] = spawnedEnemy;
                break;
            }
        }
        GameObject armorEffect = Instantiate(armourEffect, spawnedEnemy.transform.position, Quaternion.identity);
        armorEffect.GetComponent<EnemyShieldEffect>().trackObject = spawnedEnemy;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        Destroy(this.gameObject, 14f / 12f);
        StartCoroutine(spawnSkele());
    }

    void Update()
    {
        pickRendererLayer();
    }
}
