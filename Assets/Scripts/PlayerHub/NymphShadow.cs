using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NymphShadow : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    bool fadeOut = false;

    public void Initialize(Vector3 from, Vector3 to)
    {
        transform.position = from;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(to.y - from.y, to.x - from.x) * Mathf.Rad2Deg);
        fadeOut = false;
        spriteRenderer.color = new Color(1, 1, 1, 0);
        float distance = Vector2.Distance(from, to);
        float time = distance / 2;
        LeanTween.alpha(this.gameObject, 0.3f, time * 0.4f);
        LeanTween.move(this.gameObject, to, time).setOnUpdate((float val) => { if (val > 0.8f && fadeOut == false) { fadeOut = true; LeanTween.alpha(this.gameObject, 0f, time * 0.2f); } }).setOnComplete(() => Destroy(this.gameObject));
    }
}
