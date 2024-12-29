using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class BottleStatsController : UIView
{
    List<BottleData> _allBottles;
    StatFieldController _rotationForce, _dampForce, _bottleDiameter, _capDiameter, _frictionForce, _multiplier;

    public const string k_RotationForceName = "rotation-force";
    public const string k_DampForceName = "damp-force";
    public const string k_FrictionForceName = "friction-force";
    public const string k_BottleDiameterName = "bottle-diameter";
    public const string k_CapDiameterName = "cap-diameter";
    public const string k_MultiplierName = "multiplier";

    public BottleStatsController(VisualElement topElement, List<BottleData> bottleDataList) : base(topElement, false) 
    {
        _allBottles = bottleDataList;
        MenuEvents.BottleSelectionChanged += UpdateStats;
        MenuEvents.BottleEquipped += UpdateStats;
        SetUpFieldControllers();
    }

    public override void Dispose()
    {
        base.Dispose();
        MenuEvents.BottleSelectionChanged -= UpdateStats;
        MenuEvents.BottleEquipped -= UpdateStats;
    }

    private void SetUpFieldControllers()
    {
        float min, max;
        min = BottleDataHelper.GetRotationForces(_allBottles).Min();
        max = BottleDataHelper.GetRotationForces(_allBottles).Max();
        _rotationForce = new StatFieldController(_topElement.Q<VisualElement>(k_RotationForceName), min, max, "Rotation: ");

        min = BottleDataHelper.GetDampForces(_allBottles).Min();
        max = BottleDataHelper.GetDampForces(_allBottles).Max();
        _dampForce = new StatFieldController(_topElement.Q<VisualElement>(k_DampForceName), min, max, "Damp: ");

        min = BottleDataHelper.GetFrictionForces(_allBottles).Min();
        max = BottleDataHelper.GetFrictionForces(_allBottles).Max();
        _frictionForce = new StatFieldController(_topElement.Q<VisualElement>(k_FrictionForceName), min, max, "Friction: ");

        min = BottleDataHelper.GetBottleDiameters(_allBottles).Min();
        max = BottleDataHelper.GetBottleDiameters(_allBottles).Max();
        _bottleDiameter = new StatFieldController(_topElement.Q<VisualElement>(k_BottleDiameterName), min, max, "Diameter: ");

        min = BottleDataHelper.GetCapDiameters(_allBottles).Min();
        max = BottleDataHelper.GetCapDiameters(_allBottles).Max();
        _capDiameter = new StatFieldController(_topElement.Q<VisualElement>(k_CapDiameterName), min, max, "Cap diameter: ");

        min = BottleDataHelper.GetCapDiameters(_allBottles).Min();
        max = BottleDataHelper.GetCapDiameters(_allBottles).Max();
        _multiplier = new StatFieldController(_topElement.Q<VisualElement>(k_MultiplierName), min, max, "Multiplier: ");
    }

    public void UpdateStats(BottleData data)
    {
        _rotationForce.SetValue(data.RotationForce);
        _dampForce.SetValue(data.DampForce);
        _frictionForce.SetValue(data.FrictionForce);
        _bottleDiameter.SetValue(data.BottleDiameter);
        _capDiameter.SetValue(data.CapDiameter);
        _multiplier.SetValue(data.Multiplier);
    }
}
