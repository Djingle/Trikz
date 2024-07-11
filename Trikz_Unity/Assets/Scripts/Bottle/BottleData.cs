using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BottleData : ScriptableObject
{
    public string Name;
    public BottleType Type;
    public float RotationForce;
    public float DampingFactor;
    public float ComY;
    public float BottleDiameter;
    public float CapDiameter;
    public float Friction;
    public float Multiplier;
    public Mesh Model;
    public List<Material> Materials;
}
public enum BottleType
{
    Cube = 0,
    Water = 1,
    Green = 2
}
