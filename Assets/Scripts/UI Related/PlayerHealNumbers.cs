using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealNumbers : MonoBehaviour
{
    Text text;
    float alphaVal;
    Image icon;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        icon = GetComponentInChildren<Image>();
        alphaVal = 0;
    }

    public void showHealing(int healingAmount, int shipMaxHealth)
    {
        text.text = healingAmount.ToString();
        alphaVal = 1;
        text.color = new Color(0, 1, 0, alphaVal);
        icon.color = new Color(1, 1, 1, alphaVal);
        foreach (Animator animator in GetComponentsInChildren<Animator>())
        {
            animator.SetTrigger("Popup");
        }
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
        transform.position = Camera.main.WorldToScreenPoint(FindObjectOfType<PlayerScript>().transform.position + Vector3.up * 1.5f);
        if (text.color.a > 0)
        {
            alphaVal -= Time.deltaTime;
            text.color = new Color(0, 1, 0, alphaVal);
            icon.color = new Color(1, 1, 1, alphaVal);
        }
        else
        {
            alphaVal = 0;
            text.color = new Color(0, 1, 0, 0);
            icon.color = new Color(1, 1, 1, 0);
        }
    }
}
