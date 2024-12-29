using System.Collections.Generic;
using UnityEngine;

public static class BottleDataHelper
{
    public static List<float> GetRotationForces(List<BottleData> bottleDatas)
    {
        List<float> res = new List<float>();
        foreach (var bottle in bottleDatas) {
            res.Add(bottle.RotationForce);
        }
        return res;
    }
    public static List<float> GetDampForces(List<BottleData> bottleDatas)
    {
        List<float> res = new List<float>();
        foreach (var bottle in bottleDatas) {
            res.Add(bottle.DampForce);
        }
        return res;
    }
    public static List<float> GetFrictionForces(List<BottleData> bottleDatas)
    {
        List<float> res = new List<float>();
        foreach (var bottle in bottleDatas) {
            res.Add(bottle.FrictionForce);
        }
        return res;
    }
    public static List<float> GetBottleDiameters(List<BottleData> bottleDatas)
    {
        List<float> res = new List<float>();
        foreach (var bottle in bottleDatas) {
            res.Add(bottle.BottleDiameter);
        }
        return res;
    }
    public static List<float> GetCapDiameters(List<BottleData> bottleDatas)
    {
        List<float> res = new List<float>();
        foreach (var bottle in bottleDatas) {
            res.Add(bottle.CapDiameter);
        }
        return res;
    }
    public static List<float> GetMultipliers(List<BottleData> bottleDatas)
    {   
        List<float> res = new List<float>();
        foreach (var bottle in bottleDatas) {
            res.Add(bottle.Multiplier);
        }
        return res;
    }

    public static List<BottleData> AllBottles()
    {
        List<BottleData> allBottles = new List<BottleData>();
        allBottles.AddRange(Resources.LoadAll<BottleData>("Bottles"));
        return allBottles;
    }

    public static bool IsBought(BottleData bottle)
    {
        return SaveManager.Instance.State.BoughtBottles.Contains(bottle);
    }
}
