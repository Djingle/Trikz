using UnityEngine.UIElements;

public class BottleListElementController
{
    Label _label;

    public void SetVisualElement(VisualElement visualElement)
    {
        _label = visualElement.Q<Label>("bottle-name");
    }

    public void SetBottleData(BottleData bottleData)
    {
        _label.text = bottleData.BottleName;
    }
}
