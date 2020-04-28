using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GeneralSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    private UnityAction<DisplayItem> onHoverAction;
    private UnityAction<DisplayItem> onUnHoverAction;
    private UnityAction<DisplayItem> onClickAction;
    private DisplayItem displayItem;

    public void Initialize(DisplayItem displayItem, UnityAction<DisplayItem> onHoverAction, UnityAction<DisplayItem> onUnHoverAction, UnityAction<DisplayItem> onClickAction)
    {
        this.displayItem = displayItem;
        this.icon.sprite = displayItem.displayIcon;
        this.onHoverAction = onHoverAction;
        this.onUnHoverAction = onUnHoverAction;
        this.onClickAction = onClickAction;
    }

    public void ButtonAction()
    {
        onClickAction.Invoke(this.displayItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onUnHoverAction.Invoke(this.displayItem);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHoverAction.Invoke(this.displayItem);
    }
}
