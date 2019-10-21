using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {



    public void shakeCamFunction(float duration, float magnitude)
    {
        StartCoroutine(shakeCam(duration, magnitude));
    }

    IEnumerator shakeCam(float duration, float magnitude)
    {
        Vector3 origPosition;
        if (GetComponent<MoveCameraNextRoom>().trackPlayer == false)
        {
            origPosition = new Vector3(Mathf.RoundToInt(transform.position.x / 20f) * 20, Mathf.RoundToInt(transform.position.y / 20f) * 20);
        }
        else
        {
            origPosition = transform.position;
        }
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = origPosition.x + Random.Range(-1f, 1f) * magnitude;
            float y = origPosition.y + Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, origPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = origPosition;
    }
}
