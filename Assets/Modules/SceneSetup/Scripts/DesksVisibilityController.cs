using UnityEngine;
using Zenject;

public class DesksVisibilityController : MonoBehaviour
{
    [Inject] private IGameModel _gameModel;
    [Inject] private ISceneSetupModel _sceneSetupModel;

    private void Awake()
    {
        _gameModel.XRModeChanged += OnXRModeChanged;
    }

    private void OnDestroy()
    {
        _gameModel.XRModeChanged -= OnXRModeChanged;
    }

    private void OnXRModeChanged(XRMode xrMode)
    {
        foreach (var desk in _sceneSetupModel.Desks)
        {
            if (xrMode == XRMode.ARMode)
            {
                desk.Hide();
            }
            else
            {
                desk.Show();
            }
        }
    }
}
