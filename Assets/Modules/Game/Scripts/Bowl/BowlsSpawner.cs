using System.Collections;
using UnityEngine;
using Zenject;

public class BowlsSpawner : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Inject] private IGameModel _gameModel;
    [Inject] private BowlFactory _bowlsFactory;
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
        var bowl = _bowlsFactory.Create(desk);
        yield return _waitForEndOfFrame;
        bowl.transform.parent = desk.Transform;
        bowl.transform.localPosition = desk.GetBowlSpawningLocalPosition();
        bowl.transform.parent = null;
        _gameModel.AddBowl(desk, bowl);
    }
}
