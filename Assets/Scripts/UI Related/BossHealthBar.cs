using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    Image bossHealthImage;
    public Enemy targetEnemy;
    public int enemyMaxHealth;
    Text text;

    void Start()
    {
        bossHealthImage = this.GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

    public void bossStartUp(string bossName)
    {
        text.text = bossName;
        MiscData.seenEnemies.Add(bossName);
        foreach (Animator animator in GetComponentsInChildren<Animator>())
        {
            animator.SetTrigger("FadeOut");
        }
    }

    public void bossEnd()
    {
        foreach (Animator animator in GetComponentsInChildren<Animator>())
        {
            animator.SetTrigger("FadeIn");
            animator.SetTrigger("FadeIn");
        }
    }

    void Update()
    {
        if(targetEnemy != null)
        {
            bossHealthImage.fillAmount = (float)targetEnemy.health / targetEnemy.maxHealth;
        }
    }
}
