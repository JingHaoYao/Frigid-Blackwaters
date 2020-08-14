using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInActiveAfterTime : MonoBehaviour
{
    [SerializeField] float timeUntilUnActive = 0;

    IEnumerator waitUntilUnActive()
    {
        yield return new WaitForSeconds(timeUntilUnActive);

        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(waitUntilUnActive());
    }
}
