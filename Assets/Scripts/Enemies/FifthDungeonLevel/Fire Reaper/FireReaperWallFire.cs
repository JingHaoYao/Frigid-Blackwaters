using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireReaperWallFire : MonoBehaviour
{
    [SerializeField] Collider2D collider;
    [SerializeField] ProjectileParent projectileParent;

    public void Initialize(GameObject parentBoss)
    {
        StartCoroutine(startUpRoutine());
        this.projectileParent.instantiater = parentBoss;
    }
         
    IEnumerator startUpRoutine()
    {
        collider.enabled = false;

        yield return new WaitForSeconds(6 / 12f);

        collider.enabled = true;
    }

    public void FadeOut()
    {
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject));
    }
}
