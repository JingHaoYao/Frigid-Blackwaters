using UnityEngine;

public class IllusionSoulOutline : MonoBehaviour
{
    void Start()
    {
        LeanTween.alpha(this.gameObject, 0, 1f).setOnComplete(() => Destroy(this.gameObject));
    }
}
