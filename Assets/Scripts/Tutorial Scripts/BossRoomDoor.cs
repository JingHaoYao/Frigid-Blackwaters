using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossRoomDoor : MonoBehaviour
{
    [SerializeField] GameObject leftDoor, rightDoor;
    [SerializeField] CameraShake cameraShake;

    IEnumerator OpenDoor(UnityAction endAction)
    {
        cameraShake.shakeCamFunction(0.5f, 0.1f);

        yield return new WaitForSeconds(0.5f);

        LeanTween.move(leftDoor, leftDoor.transform.position + Vector3.left * 3, 1f).setEaseOutQuad();
        LeanTween.move(rightDoor, rightDoor.transform.position + Vector3.right * 3, 1f).setEaseOutQuad();

        yield return new WaitForSeconds(1f);
        endAction.Invoke();
    }

    public void Open(UnityAction endAction)
    {
        StartCoroutine(OpenDoor(endAction));
    }
}
