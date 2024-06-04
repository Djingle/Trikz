using UnityEngine;
using UnityEngine.UIElements;
using System;

public class UI : MonoBehaviour
{
    public VisualTreeAsset _winScreenTemplate;
    public VisualTreeAsset _loseScreenTemplate;
    private TemplateContainer _endScreenContainer;

    private VisualElement _root;

    private Label _totalPoints, _currentPot;
    private Button _continueButton, _withdrawButton, _replayButton;

    public static event Action continueClicked;
    public static event Action withdrawClicked;
    public static event Action replayClicked;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _totalPoints = _root.Q("total-points-value") as Label;
        _currentPot = _root.Q("current-pot-value") as Label;
        _currentPot.text = "0";
        _totalPoints.text = "0";
    }

    private void OnEnable()
    {
        GameController.totalPointsChanged += updateTotalPoints;
        GameController.currentPotChanged += updateCurrentPot;
        GameController.launchEnded += showEndScreen;
    }

    private void OnDisable()
    {
        GameController.totalPointsChanged -= updateTotalPoints;
        GameController.currentPotChanged -= updateCurrentPot;
        GameController.launchEnded -= showEndScreen;

    }

    private void updateTotalPoints(int newPoints)
    {
        _totalPoints.text = newPoints.ToString();
    }

    private void updateCurrentPot(int newPot)
    {
        _currentPot.text = newPot.ToString();
    }

    private void showEndScreen(bool win)
    {
        if (win)
        {
            _endScreenContainer = _winScreenTemplate.Instantiate();
            _root.Q("end-screen-container").Add(_endScreenContainer);
            _continueButton = _root.Q("continue-button") as Button;
            _withdrawButton = _root.Q("withdraw-button") as Button;
            _continueButton.RegisterCallback<ClickEvent>(onContinue);
            _withdrawButton.RegisterCallback<ClickEvent>(onWithdraw);
        }
        else
        {
            _endScreenContainer = _loseScreenTemplate.Instantiate();
            _root.Q("end-screen-container").Add(_endScreenContainer);
            _replayButton = _root.Q("replay-button") as Button;
            _replayButton.RegisterCallback<ClickEvent>(onReplay);
        }
    }

    public void onContinue(ClickEvent evt)
    {
        continueClicked.Invoke();
        _continueButton.UnregisterCallback<ClickEvent>(onContinue);
        _withdrawButton.UnregisterCallback<ClickEvent>(onWithdraw);
        _root.Q("end-screen-container").Clear();
    }

    public void onWithdraw(ClickEvent evt)
    {
        withdrawClicked.Invoke();
        _continueButton.UnregisterCallback<ClickEvent>(onContinue);
        _withdrawButton.UnregisterCallback<ClickEvent>(onWithdraw);
        _root.Q("end-screen-container").Clear();

    }

    public void onReplay(ClickEvent evt)
    {
        replayClicked.Invoke();
        _replayButton.UnregisterCallback<ClickEvent>(onReplay);
        _root.Q("end-screen-container").Clear();

    }
}
