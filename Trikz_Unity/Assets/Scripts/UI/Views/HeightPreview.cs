using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HeightPreview : MonoBehaviour {
	private VisualElement _root;
	private HeightProgress _preview;

	private void Start() {
		UIDocument ui = GetComponent<UIDocument>();
		_root = ui.rootVisualElement;
		if (_root == null) Debug.Log("Root not found");
		_preview = new HeightProgress();
	}

	private void OnEnable() {
		LaunchEvents.HeightChanged += OnStrengthChanged;

		GameManager.StateChanged += OnStateChanged;
	}

	private void OnDisable() {
		LaunchEvents.HeightChanged -= OnStrengthChanged;

		GameManager.StateChanged -= OnStateChanged;
	}


	private void OnStrengthChanged(float value) {
		_preview.progress = value;
		Debug.Log("value : " + value);
	}


	private void OnStateChanged(GameState newState) {
		if (newState == GameState.Launching) {
			_root.Add(_preview);
		} else if (newState == GameState.Rotating) {
			_preview.progress = 0f;
			_root.Remove(_preview);
		}
	}
}
