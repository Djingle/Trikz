using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndView : UIView
{
    private Button _mainMenuButton;

    public const string k_MainMenuButtonName = "main-menu-button";
    public EndView(VisualElement topElement) : base(topElement) { }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _mainMenuButton = _topElement.Q<Button>(k_MainMenuButtonName);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        _mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuClicked);
    }

    protected override void UnRegisterCallbacks()
    {
        base.RegisterCallbacks();
        _mainMenuButton.UnregisterCallback<ClickEvent>(OnMainMenuClicked);
    }
    private void OnMainMenuClicked(ClickEvent evt)
    {
        GameManager.Instance.ChangeState(GameState.Menu);
    }
}
