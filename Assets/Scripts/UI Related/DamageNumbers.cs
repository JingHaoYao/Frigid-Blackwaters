using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
    Text text;
    float alphaVal;
    private int tweenID;
    private int previousDamageAmount;
    Coroutine coolDownRoutine;

    void Start()
    {
        text = GetComponent<Text>();
        text.color = new Color(200, 0, 0, 0);
        alphaVal = 0;
    }

    IEnumerator coolDown()
    {
        yield return new WaitForSeconds(0.5f);
        previousDamageAmount = 0;
    }

    public void showDamage(int damageAmount, int shipMaxHealth, Vector3 pos)
    {
        LeanTween.cancel(this.gameObject);
        transform.position = Camera.main.WorldToScreenPoint(pos);
        text.text = (previousDamageAmount + damageAmount).ToString();
        previousDamageAmount += damageAmount;
        if(coolDownRoutine != null)
        {
            StopCoroutine(coolDownRoutine);
        }
        coolDownRoutine = StartCoroutine(coolDown());
        text.color = new Color(100 + 150 * (damageAmount / shipMaxHealth), 0, 0, 1);
        text.fontSize = 30 + Mathf.RoundToInt((24 * ((float)damageAmount / shipMaxHealth)));
        GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f);
        tweenID = LeanTween.scale(this.gameObject, new Vector3(1, 1), 0.25f).setEaseInOutBounce().id;
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
            text.color = new Color(200, 0, 0, alphaVal);
        }
        else
        {
            alphaVal = 0;
            text.color = new Color(200, 0, 0, 0);
        }
    }
}
