using UnityEngine;
using UnityEngine.UIElements;

public class RotatePreview : MonoBehaviour
{
    private VisualElement _root;
    private RadialProgress _preview;

    private void Start()
    {
        UIDocument ui = GetComponent<UIDocument>();
        _root = ui.rootVisualElement;
        if (_root == null) Debug.Log("Root not found");
        _preview = new RadialProgress();
        //_root.Add(_preview);
    }

    private void OnEnable()
    {
        RotateScreenScript.StrengthChanged += OnStrengthChanged;
        InputController.PhaseChanged += OnPhaseChanged;
    }

    private void OnDisable()
    {
        RotateScreenScript.StrengthChanged -= OnStrengthChanged;
        InputController.PhaseChanged += OnPhaseChanged;
    }


    private void OnStrengthChanged(float value)
    {
        _preview.progress = -value;
    }


    private void OnPhaseChanged(InputController.LaunchState launchState)
    {
        if (launchState == InputController.LaunchState.Rotating)
        {
            _root.Add(_preview);
            Debug.Log("phase is rotatig");
        }
        else if (launchState == InputController.LaunchState.Damping)
        {
            _root.Remove(_preview);
            Debug.Log("phase is dampig");
        }
        Debug.Log("oui");
    }
}
