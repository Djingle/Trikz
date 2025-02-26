using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    UIDocument _document;

    UIView _currentView;
    UIView _launchView, _rotateView, _dampView, _winView, _loseView,
           _scoresView, _mainMenuView, _bottleMenuView, _wSpaceUiView;

    [SerializeField] VisualTreeAsset _bottleListElementTemplate;

    // Control screen names
    public const string k_LaunchScreenName = "LaunchScreen";
    public const string k_RotateScreenName = "RotateScreen";
    public const string k_DampScreenName = "DampScreen";
    // Launch end screen names
    public const string k_WinScreenName = "WinScreen";
    public const string k_LoseScreenName = "LoseScreen";
    // Header screen names
    public const string k_ScoresViewName = "ScoresScreen";
    // Menu screen names
    public const string k_MainMenuName = "MainMenuScreen";
    public const string k_BottleMenuName = "BottleScreen";

    void OnEnable()
    {
        _document = GetComponent<UIDocument>();

        SetupViews();

        GameManager.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.StateChanged -= OnStateChanged;
    }

    private void SetupViews()
    {
        VisualElement root = _document.rootVisualElement;

        // Control views
        _launchView = new LaunchView(root.Q<VisualElement>(k_LaunchScreenName));
        _rotateView = new RotateView(root.Q<VisualElement>(k_RotateScreenName));
        _dampView = new DampView(root.Q<VisualElement>(k_DampScreenName));
        // Launch end views
        _winView = new WinView(root.Q<VisualElement>(k_WinScreenName));
        _loseView = new LoseView(root.Q<VisualElement>(k_LoseScreenName));
        // Header views
        _scoresView = new ScoresView(root.Q<VisualElement>(k_ScoresViewName));
        // Menu views
        _mainMenuView = new MainMenuView(root.Q<VisualElement>(k_MainMenuName));
        _bottleMenuView = new BottleListView(root.Q<VisualElement>(k_BottleMenuName), _bottleListElementTemplate);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        else {
            Instance = this;
        }
    }

    private void ShowView(UIView newView)
    {
        if (_currentView != null) {
            _currentView.Hide();
        }

        _currentView = newView;

        if (_currentView != null) {
            _currentView.Show();
        }
    }

    private void OnStateChanged(GameState newState)
    {
        switch (newState) {
            case GameState.Launching:
                ShowView(_launchView);
                _loseView.Hide();
                _winView.Hide();
                break;
            case GameState.Rotating:
                ShowView(_rotateView);
                break;
            case GameState.Damping:
                ShowView(_dampView);
                break;
            case GameState.Win:
                ShowView(_launchView);
                _winView.Show();
                break;
            case GameState.Lose:
                ShowView(_launchView);
                _loseView.Show();
                break;
            case GameState.Menu:
                ShowView(_mainMenuView);
                _scoresView.Show();
                _loseView.Hide();
                _winView.Hide();
                break;
            case GameState.BottleMenu:
                ShowView(_bottleMenuView);
                break;
        }
    }
}
