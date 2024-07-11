using UnityEngine;
using UnityEngine.UIElements;

public class ControlView : UIView
{
    protected float _controlZoneSize;
    protected Vector3 _clickPos;
    protected bool _clickRelativeControl = true;

    public bool ClickRelativeControl => _clickRelativeControl;

    public const string k_ControlZoneName = "control-zone";

    protected VisualElement _controlZone;
    public ControlView(VisualElement topElement) : base(topElement)
    {
        _topElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        InitControlSize();
    }
    protected virtual void InitControlSize() { }
    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _controlZone = _topElement.Q(k_ControlZoneName);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        _controlZone.RegisterCallback<PointerDownEvent>(OnPointerDown);
        _controlZone.RegisterCallback<PointerUpEvent>(OnPointerUp);
        _controlZone.RegisterCallback<PointerMoveEvent>(OnPointerMove);
    }
    protected override void UnRegisterCallbacks()
    {
        base.RegisterCallbacks();
        _controlZone.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        _controlZone.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        _controlZone.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
    }

    public override void Dispose()
    {
        base.Dispose();
        _topElement.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    protected virtual float RemapValue(float value)
    {
        return Mathf.InverseLerp(0, _controlZoneSize, value);
    }

    protected virtual void OnPointerDown(PointerDownEvent evt)
    {
        _clickPos = evt.localPosition;
    }
    protected virtual void OnPointerUp(PointerUpEvent evt) { }
    protected virtual void OnPointerMove(PointerMoveEvent evt) { }

}
