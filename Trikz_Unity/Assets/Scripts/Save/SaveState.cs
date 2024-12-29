using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class SaveState
{
    public SaveState()
    {
        //List<BottleData> defaultBought = new List<BottleData>{GameManager.Instance.EquippedBottle};
        //Debug.Log("defaultBought[0]: " + defaultBought[0]);
        //WriteState(GameManager.Instance.TotalPoints, GameManager.Instance.EquippedBottle, defaultBought);
    }

    public int _points;
    public string _equippedBottleName;
    public List<string> _boughtBottlesNames;

    [XmlIgnore]
    public int Points { get => _points; set => _points = value; }

    [XmlIgnore]
    public BottleData EquippedBottle
    {
        get
        {
            // Check which bottle the saved name corresponds to
            foreach (BottleData bottle in BottleDataHelper.AllBottles()) {
                if (bottle.BottleName ==_equippedBottleName) { // If we find the right one, break
                    return bottle;
                }
            }
            return null;
        }

        set
        {
            _equippedBottleName = value.BottleName;
        }
    }

    [XmlIgnore]
    public List<BottleData> BoughtBottles
    {
        get
        {
            List<BottleData> boughtBottles = new List<BottleData>();
            foreach (BottleData bottle in BottleDataHelper.AllBottles()) {
                foreach (string bottleName in _boughtBottlesNames) {
                    if (bottle.BottleName == bottleName) {
                        boughtBottles.Add(bottle);
                        break;
                    }
                }
            }
            return boughtBottles;
        }
        set
        {
            _boughtBottlesNames = new List<string>();
            if (value == null) return;
            foreach (BottleData bottle in value) {
                Debug.Log("set boughtbottles: " + bottle.BottleName);
                _boughtBottlesNames.Add(bottle.BottleName);
            }
        }
    }

    public void BuyBottle(BottleData bottle)
    {
        Debug.Log("savestate: Buying");
        _boughtBottlesNames.Add(bottle.BottleName);
        MenuEvents.TransactionConfirmed?.Invoke(bottle);
    }

    public void WriteState(int newPoints, BottleData newEquippedBottle, List<BottleData> newBoughtBottles)
    {
        Points = newPoints;
        if (newEquippedBottle != null) EquippedBottle = newEquippedBottle;
        if (newBoughtBottles != null) BoughtBottles = newBoughtBottles;
    }

    
}
