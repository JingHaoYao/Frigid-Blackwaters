using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour {
    private MoveCameraNextRoom cameraScript;
    Coroutine previousShakeRoutine;

    private void Awake()
    {
        PlayerProperties.cameraShake = this;
    }

    private void Start()
    {
        cameraScript = GetComponent<MoveCameraNextRoom>();
    }

    public void shakeCamFunction(float duration, float magnitude, UnityAction endAction = null)
    {
        if (previousShakeRoutine != null)
        {
            StopCoroutine(previousShakeRoutine);
        }
        previousShakeRoutine = StartCoroutine(shakeCam(duration, magnitude, endAction));
    }

    IEnumerator shakeCam(float duration, float magnitude, UnityAction endAction = null)
    {
        Vector3 origPosition;
        if (cameraScript.trackPlayer == false)
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
            if (cameraScript.trackPlayer == false)
            {
                float x = origPosition.x + Random.Range(-1f, 1f) * magnitude;
                float y = origPosition.y + Random.Range(-1f, 1f) * magnitude;
                transform.localPosition = new Vector3(x, y, origPosition.z);
            }
            else
            {
                Vector3 pos = cameraScript.returnTrackCamPosition();
                float x = pos.x + Random.Range(-1f, 1f) * magnitude;
                float y = pos.y + Random.Range(-1f, 1f) * magnitude;
                transform.localPosition = new Vector3(x, y, pos.z);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (cameraScript.trackPlayer == false)
        {
            transform.localPosition = origPosition;
        }
        else
        {
            transform.localPosition = cameraScript.returnTrackCamPosition();
        }

        endAction?.Invoke();
    }
}
