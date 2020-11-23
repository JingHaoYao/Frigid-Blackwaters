using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejuvenatingCrystalPlant : MonoBehaviour
{
    int phase = 1;
    [SerializeField] Sprite[] growthSprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Collider2D col;

    public void growPlant()
    {
        if (phase < 5)
        {
            phase++;
            col.enabled = true;
            spriteRenderer.sprite = growthSprites[phase - 1];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9 && collision.gameObject.tag == "playerHitBox")
        {
            LeanTween.alpha(this.gameObject, 0, 1f);
            PlayerProperties.playerScript.healPlayer(phase * 1000);
            col.enabled = false;
        }
    }
}
