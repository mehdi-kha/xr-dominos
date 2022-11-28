using UnityEngine;
using Zenject;

class NonPlayableDominoFallingDetectorSpawner : MonoBehaviour
{
    [Inject] private NonPlayableDominoFallingDetectorFactory _factory;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Tooltip("The y offset applied to the non playable domino falling detector.")]
    [SerializeField] float _detectorHeightOffset = 0.09f;

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
        nonPlayableDominoFallingDetector.transform.SetParent(deskController.Transform, false);
        nonPlayableDominoFallingDetector.transform.localPosition = Vector3.zero;
        nonPlayableDominoFallingDetector.transform.localRotation = Quaternion.identity;
        var detectorBoxCollider = nonPlayableDominoFallingDetector.GetComponent<BoxCollider>();
        detectorBoxCollider.center = deskController.Bounds.center + Vector3.up * _detectorHeightOffset;
        detectorBoxCollider.size = deskController.Bounds.size;
        nonPlayableDominoFallingDetector.CorrespondingDesk = deskController;
    }
}
