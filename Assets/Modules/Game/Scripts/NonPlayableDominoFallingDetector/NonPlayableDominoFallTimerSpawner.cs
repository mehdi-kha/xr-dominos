using UnityEngine;
using Zenject;

class NonPlayableDominoFallTimerSpawner : MonoBehaviour
{
    [Inject] private NonPlayableDominoFallTimerFactory _factory;
    [Inject] private ISceneSetupModel _sceneSetupModel;

    private void Awake()
    {
        _sceneSetupModel.DeskDetected += OnDeskDetected;
    }

    private void OnDestroy()
    {
        _sceneSetupModel.DeskDetected -= OnDeskDetected;
    }

    private void OnDeskDetected(IDesk deskController)
    {
        deskController.SetupDone += () => OnSetupDone(deskController);
    }

    private void OnSetupDone(IDesk deskController)
    {
        SetupDetector(deskController);
    }

    private void SetupDetector(IDesk deskController)
    {
        var nonPlayableDominoFallingDetector = _factory.Create();
        nonPlayableDominoFallingDetector.CorrespondingDesk = deskController;
    }
}
