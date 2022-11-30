using System.Collections;
using UnityEngine;
using Zenject;

public class DominosSpawner : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Inject] private DominosFactory _dominosFactory;
    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    private void Awake()
    {
        _sceneSetupModel.DeskDetected += OnDeskDetected;
    }

    private void OnDeskDetected(IDesk desk)
    {
        StartCoroutine(SpawnBowlOnNextFrame(desk));
    }

    /// <summary>
    ///     Wait for the next frame before spawning the bowl, in order to wait for the whole Oculus SDK calls to be finished.
    /// </summary>
    /// <param name="deskController">The desk controller which bowl corresponds to.</param>
    /// <returns></returns>
    private IEnumerator SpawnBowlOnNextFrame(IDesk desk)
    {
        var dominosController = _dominosFactory.Create(desk);
        yield return _waitForEndOfFrame;
        dominosController.transform.parent = desk.Transform;
        dominosController.transform.localPosition = desk.GetBowlSpawningLocalPosition();
        dominosController.transform.parent = null;
    }
}
