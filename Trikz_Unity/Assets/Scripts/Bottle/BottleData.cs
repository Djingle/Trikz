using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BottleData : ScriptableObject
{
    public string BottleName;
    public BottleType Type;
    public float RotationForce;
    public float DampForce;
    public float ComY;
    public float BottleDiameter;
    public float CapDiameter;
    public float FrictionForce;
    public float Multiplier;
    public Mesh Model;
    public List<Material> Materials;
    public int Price;
}
public enum BottleType
{
    Cube = 0,
    Water = 1,
    Green = 2
}
