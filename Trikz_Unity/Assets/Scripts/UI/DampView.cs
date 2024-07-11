using UnityEngine;
using UnityEngine.UIElements;
using System;

public class DampView : ControlView
{
    public DampView(VisualElement topElement) : base(topElement) { }

    protected override void OnPointerDown(PointerDownEvent evt)
    {
        LaunchEvents.DampToggled?.Invoke(true);
    }

    protected override void OnPointerUp(PointerUpEvent evt)
    {
        LaunchEvents.DampToggled?.Invoke(false);
    }

}