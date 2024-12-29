using UnityEngine.UIElements;

public class LoseView : EndView
{
    private Button _replayButton;

    public const string k_ReplayButton = "replay-button";

    public LoseView(VisualElement topElement) : base(topElement) { }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _replayButton = _topElement.Q(k_ReplayButton) as Button;
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        _replayButton.RegisterCallback<ClickEvent>(OnReplayClicked);
    }
    protected override void UnRegisterCallbacks()
    {
        base.RegisterCallbacks();
        _replayButton.UnregisterCallback<ClickEvent>(OnReplayClicked);
    }

    private void OnReplayClicked(ClickEvent evt)
    {
        GameManager.Instance.Replay();
    }
}
