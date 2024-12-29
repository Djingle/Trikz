using System;

public class MenuEvents
{
    public static Action<BottleData> BottleSelectionChanged;
    public static Action<BottleData> BottleEquipped;
    public static Action<BottleData> BottleBought;
    public static Action<BottleData> TransactionConfirmed;
}
