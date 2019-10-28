using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingJadePulse : MonoBehaviour
{
    CapsuleCollider2D capCol;
    public bool damaging = false;

    IEnumerator explode()
    {
        yield return new WaitForSeconds(2f / 12f);
        if (Vector2.Distance(transform.position, GameObject.Find("PlayerShip").transform.position) <= 2)
        {
            FindObjectOfType<PlayerScript>().healPlayer(100);
        }
    }

    IEnumerator explodeDamage()
    {
        capCol.enabled = false;
        yield return new WaitForSeconds(2f / 12f);
        capCol.enabled = true;
        yield return new WaitForSeconds(1f / 12f);
    }

    void Start()
    {
        capCol = GetComponent<CapsuleCollider2D>();
        Destroy(this.gameObject, 0.5f);
        if (damaging == false)
        {
            StartCoroutine(explode());
        }
        else
        {
            capCol.enabled = false;
            StartCoroutine(explodeDamage());
        }
    }
}
