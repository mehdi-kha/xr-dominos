using UnityEngine;
using Zenject;

public class BowlsSpawner : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Inject] private BowlFactory _bowlFactory;

    private void Awake()
    {
        _sceneSetupModel.DeskDetected += OnDeskDetected;
    }

    private void OnDeskDetected(DeskController deskController)
    {
        var bowlController = _bowlFactory.Create();
        bowlController.transform.parent = deskController.transform;
        // TODO improve by adding the possibility to define where the bag should spawn.
        bowlController.transform.localPosition = Vector3.zero;
    }
}
