using System.Linq;
using UnityEngine;

public class WspacePanel : MonoBehaviour
{
    public GameState[] destroyGameStates;
    public Vector3 k_LaunchableOffset = new Vector3(3,1,0);
    private Transform _target;
    private void Awake()
    {
        GameManager.StateChanged += OnStateChanged;
	}

	private void OnDestroy() {
		GameManager.StateChanged -= OnStateChanged;
	}

	private void Update()
    {
        switch (GameManager.Instance.State) {
            case GameState.Launching:
            case GameState.Rotating:
            case GameState.Damping:
            case GameState.Win:
            case GameState.Lose:
                transform.position = _target.position + k_LaunchableOffset;
                return;
        }
    }
    
    private void OnStateChanged(GameState newState) {
        if (destroyGameStates.Contains(newState)) {
            Destroy(gameObject);
        }

        if (newState == GameState.Launching) {
            if (!Launchable.Instance) Debug.LogError("WspacePanel : Pas de joueur");
            _target = Launchable.Instance.CenterOfMass.transform;
        }
    }
}
