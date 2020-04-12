using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillowNotification : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(1f, 1f);
        LeanTween.scale(this.gameObject, new Vector3(3, 3), 0.75f).setEaseOutBounce().setOnComplete(() => LeanTween.scale(this.gameObject, Vector3.zero, 0.75f).setOnComplete(() => Destroy(this.gameObject)));
    }

    private void Update()
    {
        transform.position = PlayerProperties.playerShipPosition;
    }
}
