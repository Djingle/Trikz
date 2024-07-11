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
    }

    private void OnEnable()
    {
        LaunchEvents.StrengthChanged += OnStrengthChanged;

        GameManager.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        LaunchEvents.StrengthChanged -= OnStrengthChanged;

        GameManager.StateChanged -= OnStateChanged;
    }


    private void OnStrengthChanged(float value)
    {
        _preview.progress = value-.5f;
    }


    private void OnStateChanged(GameState newState)
    {
        if (newState == GameState.Rotating)
        {
            _root.Add(_preview);
        }
        else if (newState == GameState.Damping)
        {
            _preview.progress = 0f;
            _root.Remove(_preview);
        }
    }
}
