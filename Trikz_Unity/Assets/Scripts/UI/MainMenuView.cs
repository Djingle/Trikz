using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuView : UIView
{
    private Button _playButton, _bottlesButton, _optionsButton;
    public MainMenuView(VisualElement topElement) : base(topElement) { }

    public const string k_PlayButtonName = "play-button";
    public const string k_BottlesButtonName = "bottles-button";
    public const string k_OptionsButtonName = "options-button";

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _playButton    = _topElement.Q<Button>(k_PlayButtonName);
        _bottlesButton = _topElement.Q<Button>( k_BottlesButtonName);
        _optionsButton = _topElement.Q<Button>(k_OptionsButtonName);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        _playButton.RegisterCallback<ClickEvent>(OnPlayClicked);
        _bottlesButton.RegisterCallback<ClickEvent>(OnBottlesClicked);
        _optionsButton.RegisterCallback<ClickEvent>(OnOptionsClicked);
    }

    protected override void UnRegisterCallbacks()
    {
        base.RegisterCallbacks();
        _playButton.UnregisterCallback<ClickEvent>(OnPlayClicked);
        _bottlesButton.UnregisterCallback<ClickEvent>(OnBottlesClicked);
        _optionsButton.UnregisterCallback<ClickEvent>(OnOptionsClicked);
    }

    private void OnPlayClicked(ClickEvent evt)
    {
        GameManager.Instance.ChangeState(GameState.Launching);
    }

    private void OnBottlesClicked(ClickEvent evt)
    {
        Debug.Log("main menu : bottles clicked");
        GameManager.Instance.ChangeState(GameState.BottleMenu);
    }

    private void OnOptionsClicked(ClickEvent evt)
    {
        GameManager.Instance.ChangeState(GameState.OptionsMenu);
    }
}
