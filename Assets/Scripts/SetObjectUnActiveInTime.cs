using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectUnActiveInTime : MonoBehaviour
{
    public float timeUntilUnActive;
    void OnEnable()
    {
        StartCoroutine(unactive());
    }

    IEnumerator unactive()
    {
        yield return new WaitForSeconds(timeUntilUnActive);
        this.gameObject.SetActive(false);
    }
    
}
