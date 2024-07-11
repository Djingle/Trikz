using System;

public static class ScoreEvents
{
    public static Action<int> TotalPointsChanged;
    public static Action<int> CurrentPotChanged;
    public static Action<int> HighScoreChanged;
    public static Action<float> MultiplierChanged;
}
