using UnityEngine;
using UnityEngine.UIElements;

public class WorldSpaceUI : MonoBehaviour
{
	private Label _currentPot, _multiplier;

	public const string k_CurrentPotName = "current-pot-value";
	public const string k_MultiplierValue = "multiplier-value";

	private VisualElement _root;

	void Awake() {
		ScoreEvents.CurrentPotChanged += OnCurrentPotChanged;
		ScoreEvents.MultiplierChanged += OnMultiplierChanged;
	}

	public void OnDestroy() {
		ScoreEvents.CurrentPotChanged -= OnCurrentPotChanged;
		ScoreEvents.MultiplierChanged -= OnMultiplierChanged;
	}

	void Start() {
		_root = GetComponent<UIDocument>().rootVisualElement;

		_currentPot = _root.Q(k_CurrentPotName) as Label;
		_multiplier = _root.Q(k_MultiplierValue) as Label;

		_currentPot.text = "0";
		_multiplier.text = "0";
	}

	private void OnCurrentPotChanged(int value) {
		_currentPot.text = value.ToString();
	}

	private void OnMultiplierChanged(float value) {
		_multiplier.text = value.ToString("0.##");
	}
}
