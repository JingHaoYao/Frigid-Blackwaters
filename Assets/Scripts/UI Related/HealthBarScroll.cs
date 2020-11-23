using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScroll : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [SerializeField] RectTransform edgeRect;
    [SerializeField] RectTransform maskRect;
    [SerializeField] Image maskImage;
    private float maskLength;

    private void Start()
    {
        maskLength = maskRect.sizeDelta.y;
    }

    private void Update()
    {
        Rect uvRect = rawImage.uvRect;
        uvRect.y -= 0.2f * Time.deltaTime;
        rawImage.uvRect = uvRect;
        edgeRect.anchoredPosition = new Vector2(0, maskImage.fillAmount * maskLength);
    }
}
