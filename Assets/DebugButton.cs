using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugButton : Button
{
    // disables color change when moused over
    public override void OnPointerEnter(PointerEventData eventData) { }

    // disables button event call
    public override void OnPointerClick(PointerEventData eventData) { }

    // disables color change when pressed
    public override void OnPointerDown(PointerEventData eventData) { }
}
