using System;
using UnityEngine.UIElements;
using UnityEngine;

public class UIView : IDisposable
{
    public PanelSettings _panelSettings;
    protected bool _hideOnAwake = true;
    protected VisualElement _topElement;
    public VisualElement Root => _topElement;
    public UIView(VisualElement topElement, bool hideOnAwake=true)
    {
        _hideOnAwake = hideOnAwake;
        _topElement = topElement ?? throw new ArgumentNullException(nameof(topElement));
        Initialize();
    }

    public virtual void Initialize()
    {
        if (_hideOnAwake) {
            Hide();
        }
        SetVisualElements();
        RegisterCallbacks();
    }

    protected virtual void SetVisualElements() { }

    protected virtual void RegisterCallbacks() { }
    protected virtual void UnRegisterCallbacks() { }

    public virtual void Show()
    {
        _topElement.style.display = DisplayStyle.Flex;
    }
    public virtual void Hide()
    {
        _topElement.style.display = DisplayStyle.None;
    }

    public virtual void Dispose()
    {
        UnRegisterCallbacks();
    }
}
