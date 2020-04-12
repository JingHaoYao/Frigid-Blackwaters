using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallinePillarSmallCrystal : MonoBehaviour
{
    [SerializeField] float timeUntilFadeAway;
    [SerializeField] Collider2D damageCollider;

    private void Start()
    {
        StartCoroutine(fadeAway());
    }

    IEnumerator fadeAway()
    {
        yield return new WaitForSeconds(timeUntilFadeAway);
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject));
        damageCollider.enabled = false;
    }
}
