using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEvents
{
    public static Action BottleButtonClicked;
    public static Action OptionsButtonClicked;
    public static Action<BottleData> BottleSelectionChanged;
    public static Action<BottleData> BottleEquipped;
}
