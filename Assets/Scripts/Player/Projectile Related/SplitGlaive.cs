using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitGlaive : MonoBehaviour
{
    [SerializeField] CircleCollider2D damagingCollider;
    public bool noDamage = false;
    public float fadeTime = 1;
    void Start()
    {
        LeanTween.alpha(this.gameObject, 0f, fadeTime).setEaseInOutCubic().setOnComplete(() => { Destroy(this.gameObject); });
        if (!noDamage)
        {
            damagingCollider.enabled = false;
            StartCoroutine(spinDamage());
        }
    }

    IEnumerator spinDamage()
    {
        while (true)
        {
            LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 0.1f);
            yield return new WaitForSeconds(0.1f);
            LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 0.1f);
            damagingCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            damagingCollider.enabled = false;
        }
    }
}
