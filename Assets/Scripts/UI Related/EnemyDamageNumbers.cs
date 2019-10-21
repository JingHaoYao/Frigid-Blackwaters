using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageNumbers : MonoBehaviour
{
    Text text;
    float alphaVal;
    GameObject trackedEnemy;

    public void showDamage(int damageAmount, GameObject enemy)
    {
        text = GetComponent<Text>();
        transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, 1.5f, 0));
        trackedEnemy = enemy;
        text.text = damageAmount.ToString();
        text.color = new Color(1, 1, 1, 1);
        alphaVal = 1;
        StartCoroutine(showDamage());
    }

    IEnumerator showDamage()
    {
        for (int i = 0; i < 2; i++)
        {
            text.enabled = false;
            yield return new WaitForSeconds(0.05f);
            text.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Update()
    {
        if (text.color.a > 0)
        {
            alphaVal -= Time.deltaTime;
            text.color = new Color(1, 1, 1, alphaVal);
            if (trackedEnemy != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(trackedEnemy.transform.position + new Vector3(0, 1.5f, 0));
            }
        }
        else
        {
            alphaVal = 0;
            text.color = new Color(1, 1, 1, 0);
            Destroy(this.gameObject);
        }
    }
}
