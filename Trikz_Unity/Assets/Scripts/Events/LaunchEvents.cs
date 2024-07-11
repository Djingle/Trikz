using System;

public static class LaunchEvents
{
    public static Action<float> HeightChanged;
    public static Action<float> HeightDecided;
    public static Action<float> StrengthChanged;
    public static Action<float> StrengthDecided;
    public static Action<bool> DampToggled;
    public static Action<bool> HasStopped;
    public static Action HasFlipped;
}
