using UnityEngine;
using Zenject;

class NonPlayableDominoFallingDetectorSpawner : MonoBehaviour
{
    [Inject] private NonPlayableDominoFallingDetectorFactory _factory;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Tooltip("The y offset applied to the non playable domino falling detector.")]
    [SerializeField] float _detectorHeightOffset = 0.09f;

    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    private void Awake()
    {
        _sceneSetupModel.DeskDetected += OnDeskDetected;
    }

    private void OnDestroy()
    {
        _sceneSetupModel.DeskDetected -= OnDeskDetected;
    }

    private void OnDeskDetected(DeskController deskController)
    {
        deskController.SetupDone += () => OnSetupDone(deskController);
    }

    private void OnSetupDone(DeskController deskController)
    {
        SetupDetector(deskController);
    }

    private void SetupDetector(DeskController deskController)
    {
        var nonPlayableDominoFallingDetector = _factory.Create();
        nonPlayableDominoFallingDetector.transform.SetParent(deskController.transform, false);
        nonPlayableDominoFallingDetector.transform.localPosition = Vector3.zero;
        nonPlayableDominoFallingDetector.transform.localRotation = Quaternion.identity;
        var detectorBoxCollider = nonPlayableDominoFallingDetector.GetComponent<BoxCollider>();
        detectorBoxCollider.center = deskController.Bounds.center + Vector3.up * _detectorHeightOffset;
        detectorBoxCollider.size = deskController.Bounds.size;
    }
}
