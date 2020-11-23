using UnityEngine;
using UnityEngine.UI;

public class ArtifactKillIndicator : MonoBehaviour
{
    [SerializeField] RectTransform maskImage;
    [SerializeField] RectTransform edgeRect;
    float maskLength;
    [SerializeField] RawImage rawImage;
    float previousFillAmount = 0;
    int tweenID = 0;


    private void Start()
    {
        maskLength = maskImage.sizeDelta.y;
        maskImage.sizeDelta = new Vector2(maskImage.sizeDelta.x, 0);
        edgeRect.anchoredPosition = new Vector2(0, 0);
    }

    void Update()
    {
        float fill = (float)PlayerProperties.playerArtifacts.numKills / PlayerProperties.playerArtifacts.killMax;
        Vector2 maskSizeDelta = maskImage.sizeDelta;
        if (Mathf.Abs(previousFillAmount - fill) > 0.01f)
        {
            float prevFill = previousFillAmount;
            LeanTween.cancel(tweenID);
            tweenID = LeanTween.value(prevFill, fill, 1.0f).setEaseOutCirc().setOnUpdate((float val) => { maskSizeDelta.y = val * maskLength; maskImage.sizeDelta = maskSizeDelta; edgeRect.anchoredPosition = new Vector2(0, val * maskLength); }).id;
            previousFillAmount = fill;
        }

        edgeRect.gameObject.SetActive(edgeRect.anchoredPosition.y / maskLength > 0 && edgeRect.anchoredPosition.y / maskLength < 1);

        Rect uvRect = rawImage.uvRect;
        uvRect.y -= 0.3f * Time.deltaTime;
        rawImage.uvRect = uvRect;

    }
}
