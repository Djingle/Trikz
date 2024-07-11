using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.UIElements;

public class ScoresView : UIView
{
    private Label _totalPoints, _currentPot, _highScore, _multiplier;

    public const string k_TotalPointsName = "total-points-value";
    public const string k_CurrentPotName = "current-pot-value";
    public const string k_HighScoreName = "high-score-value";
    public const string k_MultiplierValue = "multiplier-value";

    public ScoresView(VisualElement topElement) : base(topElement)
    {
        ScoreEvents.TotalPointsChanged += OnTotalPointsChanged;
        ScoreEvents.CurrentPotChanged += OnCurrentPotChanged;
        ScoreEvents.HighScoreChanged += OnHighScoreChanged;
        ScoreEvents.MultiplierChanged += OnMultiplierChanged;
    }

    public override void Dispose()
    {
        base.Dispose();
        ScoreEvents.TotalPointsChanged -= OnTotalPointsChanged;
        ScoreEvents.CurrentPotChanged -= OnCurrentPotChanged;
        ScoreEvents.HighScoreChanged -= OnHighScoreChanged;
        ScoreEvents.MultiplierChanged -= OnMultiplierChanged;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _totalPoints    = _topElement.Q(k_TotalPointsName) as Label;
        _currentPot     = _topElement.Q(k_CurrentPotName) as Label;
        _highScore      = _topElement.Q(k_HighScoreName) as Label;
        _multiplier = _topElement.Q(k_MultiplierValue) as Label;

        _totalPoints.text = "0";
        _currentPot.text = "0";
    }

    private void OnTotalPointsChanged(int value)
    {
        _totalPoints.text = value.ToString();
    }

    private void OnCurrentPotChanged(int value)
    {
        _currentPot.text = value.ToString();
    }

    private void OnHighScoreChanged(int value)
    {
        _highScore.text = value.ToString();
    }

    private void OnMultiplierChanged(float value)
    {
        _multiplier.text = value.ToString("0.##");
    }
}
