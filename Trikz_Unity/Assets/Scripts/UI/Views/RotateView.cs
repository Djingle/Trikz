using UnityEngine.UIElements;
using System;
using UnityEngine;

public class RotateView : ControlView
{
    public RotateView(VisualElement topElement) : base(topElement) { }

    protected override void InitControlSize()
    {
        base.InitControlSize();
        _controlZoneSize = _controlZone.layout.width;
    }

    protected override float RemapValue(float value)
    {
        if (_clickRelativeControl) return Mathf.InverseLerp(_clickPos.x - _controlZoneSize/3, _clickPos.x + _controlZoneSize / 3, value);
        else return base.RemapValue(value);
    }

    protected override void OnPointerDown(PointerDownEvent evt)
    {
        if (_clickRelativeControl) base.OnPointerDown(evt);
        else LaunchEvents.StrengthChanged?.Invoke(RemapValue(evt.localPosition.x));
    }

    protected override void OnPointerMove(PointerMoveEvent evt)
    {
        LaunchEvents.StrengthChanged?.Invoke(RemapValue(evt.localPosition.x));
    }

    protected override void OnPointerUp(PointerUpEvent evt)
    {
        LaunchEvents.StrengthDecided?.Invoke(RemapValue(evt.localPosition.x));
    }
}
