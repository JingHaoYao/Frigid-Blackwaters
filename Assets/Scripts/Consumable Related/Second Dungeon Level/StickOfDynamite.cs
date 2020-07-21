using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickOfDynamite : MonoBehaviour
{
    ConsumableBonus consumableBonus;
    PlayerScript playerScript;
    bool activated = false;
    public GameObject stickOfDynamite;

    void Start()
    {
        consumableBonus = GetComponent<ConsumableBonus>();
        playerScript = FindObjectOfType<PlayerScript>();
        consumableBonus.SetAction(throwDynamite);
    }

    void summonDynamite()
    {
        Enemy[] activeEnemies = FindObjectsOfType<Enemy>();
        if (activeEnemies.Length == 0)
        {
            GameObject stickInstant = Instantiate(stickOfDynamite, playerScript.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            int randAngle = Random.Range(0, 360);
            stickInstant.GetComponent<StickOfDynamiteProjectile>().targetPosition = new Vector3(Mathf.Cos(randAngle * Mathf.Deg2Rad), Mathf.Sin(randAngle * Mathf.Deg2Rad)) * 6;
            return;
        }

        float closestDistance = float.MaxValue;
        Enemy targetEnemy = null;
        foreach (Enemy enemy in activeEnemies)
        {
            if (Vector2.Distance(playerScript.transform.position, enemy.transform.position) < closestDistance)
            {
                closestDistance = Vector2.Distance(playerScript.transform.position, enemy.transform.position);
                targetEnemy = enemy;
            }
        }

        GameObject stick = Instantiate(stickOfDynamite, playerScript.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        stick.GetComponent<StickOfDynamiteProjectile>().targetPosition = targetEnemy.transform.position;
    }

    void throwDynamite()
    {
        if (activated == false)
        {
            activated = true;
            summonDynamite();
            Destroy(this.gameObject);
        }
    }
}
