using UnityEngine;
using UnityEngine.UIElements;

public class StatFieldController : UIView
{
    ProgressBar _progressBar;
    Image _icon;
    string _progressBarTitle;

    public const string k_ProgressBarName = "progress-bar";
    public const string k_IconName = "icon";
    //public const string k_LabelName = "label";
    public StatFieldController(VisualElement topElement, float minVal, float maxVal, string text="") : base(topElement, false) 
    {
        _progressBar.lowValue = minVal;
        _progressBar.highValue = maxVal;
        _progressBarTitle = text;
        _progressBar.title = _progressBarTitle;
        //_label.text = text;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _progressBar = _topElement.Q<ProgressBar>(k_ProgressBarName);
        //_label = _topElement.Q<Label>(k_LabelName);
        // _icon = _topElement.Q<Image>(k_IconName);
    }

    public void SetValue(float value)
    {
        _progressBar.value = value;
        _progressBar.title = _progressBarTitle + value.ToString();
        //Debug.Log(_topElement.name + ": " + value + " [" + _progressBar.lowValue + "," + _progressBar.highValue + "]");
    }
}
