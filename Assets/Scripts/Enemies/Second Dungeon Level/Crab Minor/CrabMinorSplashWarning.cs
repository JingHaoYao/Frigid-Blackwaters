using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMinorSplashWarning : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        LeanTween.alpha(this.gameObject, 1, 1.4f).setOnComplete(() => Destroy(this.gameObject));
    }
}
