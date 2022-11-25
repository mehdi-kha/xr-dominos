using UnityEngine;
using UnityEngine.Splines;
using Zenject;

public class NonPlayableDominosSpawner : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private GameObject _nonPlayableDominoPrefab;
    [SerializeField] private float _distanceBetweenDominos = 0.05f;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Range(0, 1)]
    [SerializeField] private float _holeStartRelativeDistance = 0.4f;
    [Range(0, 1)]
    [SerializeField] private float _holeEndRelativeDistance = 0.6f;

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
            var domino = Instantiate(_nonPlayableDominoPrefab);
            // TODO add a non playable tag for this domino
            domino.transform.position = worldPosition;
            domino.transform.LookAt(worldPosition + tangent, upVector);
            currentRelativePosition += _relativeDistanceBetweenDominos;
            while(currentRelativePosition > _holeStartRelativeDistance && currentRelativePosition < _holeEndRelativeDistance)
            {
                currentRelativePosition += _relativeDistanceBetweenDominos;
            }
        }
    }
}
