using UnityEngine;
using UnityEngine.UIElements;

public class WinView : EndView
{
    private Button _continueButton;
    private Button _withdrawButton;

    private const string k_ContinueButtonName = "continue-button";
    private const string k_WithdrawButtonName = "withdraw-button";
    public WinView(VisualElement topElement) : base(topElement) { }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _continueButton = _topElement.Q(k_ContinueButtonName) as Button;
        _withdrawButton = _topElement.Q(k_WithdrawButtonName) as Button;
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        _continueButton.RegisterCallback<ClickEvent>(OnContinueClicked);
        _withdrawButton.RegisterCallback<ClickEvent>(OnWithdrawClicked);
    }
    protected override void UnRegisterCallbacks()
    {
        base.UnRegisterCallbacks();
        _continueButton.UnregisterCallback<ClickEvent>(OnContinueClicked);
        _withdrawButton.UnregisterCallback<ClickEvent>(OnWithdrawClicked);
    }

    private void OnContinueClicked(ClickEvent evt)
    {
        GameManager.Instance.Continue();
    }

    private void OnWithdrawClicked(ClickEvent evt)
    {
        GameManager.Instance.Withdraw();
    }
}
