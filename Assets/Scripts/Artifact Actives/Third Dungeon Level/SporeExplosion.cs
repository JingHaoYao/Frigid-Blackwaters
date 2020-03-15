using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeExplosion : MonoBehaviour
{
    public GameObject sporeInfectionEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            GameObject sporeinstant = Instantiate(sporeInfectionEffect, enemy.transform.position, Quaternion.identity);
            enemy.addStatus(sporeinstant.GetComponent<SporeInfectionEffect>());
        }
    }
}
