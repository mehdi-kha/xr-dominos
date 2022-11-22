using System.Collections;
using UnityEngine;
using Zenject;

public class BowlsSpawner : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Inject] private BowlFactory _bowlFactory;
    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    private void Awake()
    {
        _sceneSetupModel.DeskDetected += OnDeskDetected;
    }

    private void OnDeskDetected(DeskController deskController)
    {
        StartCoroutine(SpawnBowlOnNextFrame(deskController));
    }

    /// <summary>
    ///     Wait for the next frame before spawning the bowl, in order to wait for the whole Oculus SDK calls to be finished.
    /// </summary>
    /// <param name="deskController">The desk controller which bowl corresponds to.</param>
    /// <returns></returns>
    private IEnumerator SpawnBowlOnNextFrame(DeskController deskController)
    {
        var bowlController = _bowlFactory.Create();
        yield return _waitForEndOfFrame;
        bowlController.transform.parent = deskController.transform;
        bowlController.transform.localPosition = deskController.GetBowlSpawningLocalPosition();
        bowlController.transform.parent = null;
    }
}
