using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {
    Inventory inventory;
    PlayerScript playerScript;
    Text textComponent;
    Image imageComponent;
    RectTransform rectTransform;
    RectTransform canvasRectTransform;

    private void Awake()
    {
        PlayerProperties.toolTip = this;
    }

    void Start () {
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        playerScript = FindObjectOfType<PlayerScript>();
        textComponent = GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
        canvasRectTransform = transform.parent.GetComponent<RectTransform>();
        imageComponent = GetComponent<Image>();
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || playerScript.windowAlreadyOpen == false)
        {
            this.gameObject.SetActive(false);
        }
	}

    public void SetTextAndPosition(string text, Vector3 position)
    {
        this.gameObject.SetActive(true);
        
        textComponent.text = text;

        Canvas.ForceUpdateCanvases();

        float minX = (rectTransform.sizeDelta.x - canvasRectTransform.sizeDelta.x) * 0.5f;
        float maxX = (canvasRectTransform.sizeDelta.x - rectTransform.sizeDelta.x) * 0.5f;
        float minY = (-canvasRectTransform.sizeDelta.y) * 0.5f;
        float maxY = (canvasRectTransform.sizeDelta.y * 0.5f - rectTransform.sizeDelta.y);

        rectTransform.position = new Vector3(
            Mathf.Clamp(position.x, minX + canvasRectTransform.position.x, maxX + canvasRectTransform.position.x),
            Mathf.Clamp(position.y, minY + canvasRectTransform.position.y, maxY + canvasRectTransform.position.y));
    }
}
