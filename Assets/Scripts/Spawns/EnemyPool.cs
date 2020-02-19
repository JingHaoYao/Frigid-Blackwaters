using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyPool
{
    public static List<Enemy> enemyPool = new List<Enemy>();
    public static EnemyDamageNumbersUI enemyDamageNumbers;

    public static void showDamageNumbers(int damage, Enemy enemy)
    {
        enemyDamageNumbers.addEnemyDamageUI(damage, enemy.gameObject);
    }

    public static void addEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            enemyPool.Add(enemy);
        }
    }

    public static void removeEnemy(Enemy enemy)
    {
        //logEnemyPool();
        if (enemyPool.Contains(enemy))
        {
            enemyPool.Remove(enemy);
        }
    }

    public static bool isPoolEmpty()
    {
        return enemyPool.Count == 0;
    }

    public static void logEnemyPool()
    {
        Debug.Log(enemyPool.Count);
    }
}
