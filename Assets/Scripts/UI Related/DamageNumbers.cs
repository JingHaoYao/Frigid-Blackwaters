using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
    Text text;
    float alphaVal;

    void Start()
    {
        text = GetComponent<Text>();
        text.color = new Color(200, 0, 0, 0);
        alphaVal = 0;
    }

    public void showDamage(int damageAmount, int shipMaxHealth, Vector3 pos)
    {
        transform.position = Camera.main.WorldToScreenPoint(pos);
        text.text = damageAmount.ToString();
        text.color = new Color(100 + 150 * (damageAmount / shipMaxHealth), 0, 0, 1);
        text.fontSize = 22 + Mathf.RoundToInt((24 * ((float)damageAmount / shipMaxHealth)));
        alphaVal = 1;
        GetComponent<Animator>().SetTrigger("Popup");
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
            text.color = new Color(200, 0, 0, alphaVal);
        }
        else
        {
            alphaVal = 0;
            text.color = new Color(200, 0, 0, 0);
        }
    }
}
