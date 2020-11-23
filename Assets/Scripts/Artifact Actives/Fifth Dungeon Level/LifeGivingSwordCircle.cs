using System.Collections;
using UnityEngine;

public class LifeGivingSwordCircle : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public void Initialize(float duration)
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        LeanTween.alpha(this.gameObject, 1, 1f).setEaseOutQuad();

        StartCoroutine(AnimationDuration(duration));
    }

    IEnumerator AnimationDuration(float duration)
    {
        float period = 0;
        while(period < duration)
        {
            LeanTween.alpha(this.gameObject, 0.7f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            LeanTween.alpha(this.gameObject, 1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            period += 1;
        }

        LeanTween.alpha(this.gameObject, 0, 1f).setEaseOutQuad().setOnComplete(() => Destroy(this.gameObject));
    }
}
