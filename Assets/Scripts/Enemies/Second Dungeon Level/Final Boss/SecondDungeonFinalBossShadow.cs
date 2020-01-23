using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDungeonFinalBossShadow : MonoBehaviour
{
    public void fadeShadowIn()
    {
        LeanTween.value(0, 0.075f, 1f).setOnUpdate((float val) => { transform.localScale = new Vector3(val, val); });
    }
}
