using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlammableStackUI : MonoBehaviour
{
    [SerializeField] List<Image> flammableIconStacks;
    [SerializeField] List<Animator> animators;
    [SerializeField] Sprite unactive, unactiveFinal;

    public GameObject fireyBar;
    public RectTransform fireyRectTransform;
    public Image healthBarFill;
    private float fireyBarHeight;

    Coroutine igniteInstant;

    private void Start()
    {
        fireyBarHeight = fireyRectTransform.sizeDelta.y;
    }

    public void UpdateFireyBar(int amountDamage)
    {
        if(amountDamage > 0)
        {
            fireyBar.SetActive(true);
        }
        else
        {
            fireyBar.SetActive(false);
        }
        fireyRectTransform.sizeDelta = new Vector3(fireyRectTransform.sizeDelta.x, fireyBarHeight * Mathf.Clamp((1 - healthBarFill.fillAmount) + (float)(amountDamage) / PlayerProperties.playerScript.shipHealthMAX, 0, 1));
    }

    public void IgniteStacksIconsAnimation()
    {
        if(igniteInstant != null)
        {
            StopCoroutine(igniteInstant);
        }

        igniteInstant = StartCoroutine(igniteAnimation());
    }

    IEnumerator igniteAnimation()
    {
        for (int i = 0; i < flammableIconStacks.Count; i++)
        {
            animators[i].enabled = false;
            flammableIconStacks[i].color = Color.grey;
            flammableIconStacks[i].sprite = unactive;

            if(i == 4)
            {
                flammableIconStacks[i].sprite = unactiveFinal;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void UpdateFlammableIconStacks(int numberStacks)
    {
        if (igniteInstant != null)
        {
            StopCoroutine(igniteInstant);
        }

        for (int i = 0; i < flammableIconStacks.Count; i++)
        {
            if(i < numberStacks)
            {
                flammableIconStacks[i].color = Color.white;
                animators[i].enabled = true;
                if(i == 4)
                {
                    animators[i].SetTrigger("Final");
                }
            }
            else
            {
                animators[i].enabled = false;
                flammableIconStacks[i].color = Color.grey;
                flammableIconStacks[i].sprite = unactive;

                if(i == 4)
                {
                    flammableIconStacks[i].sprite = unactiveFinal;
                }
            }
        }

        UpdateFireyBar(Mathf.RoundToInt(numberStacks * 0.1f * PlayerProperties.playerScript.shipHealthMAX));
    }
}
