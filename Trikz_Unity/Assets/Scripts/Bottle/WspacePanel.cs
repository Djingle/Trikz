using UnityEngine;

public class WspacePanel : MonoBehaviour
{
    public readonly Vector3 k_LaunchableOffset = new Vector3(3,1,0);
    private Transform _target;
    public static WspacePanel Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        } else {
            Instance = this;
        }
        _target = Launchable.Instance.CenterOfMass.transform;
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
}
