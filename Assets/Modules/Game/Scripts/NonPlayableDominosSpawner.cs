using UnityEngine;
using UnityEngine.Splines;
using Zenject;

public class NonPlayableDominosSpawner : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private GameObject _dominoPrefab;
    [SerializeField] private float _distanceBetweenDominos = 0.05f;
    [Inject] private ISceneSetupModel _sceneSetupModel;

    private float _splineLength;
    private float _relativeDistanceBetweenDominos;

    private void Awake()
    {
        _sceneSetupModel.GameStarted += OnGameStarted;
        _splineLength = _splineContainer.CalculateLength();
        _relativeDistanceBetweenDominos = _distanceBetweenDominos / _splineLength;
    }

    private void OnGameStarted()
    {
        SpawnNonPlayableDominosAlongSpline();
    }

    private void SpawnNonPlayableDominosAlongSpline()
    {
        float currentRelativePosition = 0;
        while (currentRelativePosition < 1)
        {
            _splineContainer.Evaluate(currentRelativePosition, out var worldPosition, out var tangent, out var upVector);
            var domino = Instantiate(_dominoPrefab);
            domino.transform.position = worldPosition;
            domino.transform.LookAt(worldPosition + tangent, upVector);
            currentRelativePosition += _relativeDistanceBetweenDominos;
        }
    }
}
