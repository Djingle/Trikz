using System;
using UnityEngine;

public class SaveEvents
{
    public static Action<SaveState> StateSaved;
    public static Action<SaveState> StateLoaded;
}
