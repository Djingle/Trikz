using UnityEngine.UIElements;
using UnityEngine;

public class LaunchView : ControlView
{
    public LaunchView(VisualElement topElement) : base(topElement) { }

    protected override void InitControlSize()
    {
        base.InitControlSize();
        _controlZoneSize = _controlZone.layout.height;
    }

    protected override float RemapValue(float value)
    {
        if (_clickRelativeControl) return 1 - Mathf.InverseLerp(_clickPos.y - _controlZoneSize / 2, _clickPos.y, value);
        else return 1 - base.RemapValue(value);
    }

    protected override void OnPointerDown(PointerDownEvent evt)
    {
        if (_clickRelativeControl) base.OnPointerDown(evt);
        else LaunchEvents.HeightChanged?.Invoke(RemapValue(evt.localPosition.y));
    }

    protected override void OnPointerMove(PointerMoveEvent evt)
    {
        LaunchEvents.HeightChanged?.Invoke(RemapValue(evt.localPosition.y));
    }

    protected override void OnPointerUp(PointerUpEvent evt)
    {
        LaunchEvents.HeightDecided?.Invoke(RemapValue(evt.localPosition.y));
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
    }
}
