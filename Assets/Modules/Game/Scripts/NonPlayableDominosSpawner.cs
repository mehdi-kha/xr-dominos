using System;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

public class NonPlayableDominosSpawner : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainerPrefab;
    [SerializeField] private GameObject _nonPlayableDominoPrefab;
    [SerializeField] private float _distanceBetweenDominos = 0.05f;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Range(0, 1)]
    [SerializeField] private float _holeStartRelativeDistance = 0.4f;
    [Range(0, 1)]
    [SerializeField] private float _holeEndRelativeDistance = 0.6f;
    [SerializeField] private float _verticalSpawningOffset = 0.05f;

    private float _splineLength;
    private float _relativeDistanceBetweenDominos;
    private SplineContainer _spline;

    private void Awake()
    {
        // TODO in general improve this to be more flexible and be able to spawn different types of splines
        // Maybe even randomly generate a spline on the desk?
        _sceneSetupModel.DeskDetected += OnDeskDetected;
        _sceneSetupModel.GameStarted += OnGameStarted;
        _splineLength = _splineContainerPrefab.CalculateLength();
        _relativeDistanceBetweenDominos = _distanceBetweenDominos / _splineLength;
    }

    private void OnDeskDetected(DeskController deskController)
    {
        deskController.SetupDone += () => OnDeskSetupDone(deskController);
    }

    private void OnDeskSetupDone(DeskController deskController)
    {
        // Spawn spline on top of the desk
        _spline = Instantiate(_splineContainerPrefab, deskController.transform);
        // Position the spline on top of the desk
        _spline.transform.position += deskController.CenterTopPosition - deskController.transform.position;
        // Add offset
        _spline.transform.localPosition += _verticalSpawningOffset * transform.up;
        // Adapt the spline to match the table
        _spline.transform.localScale = deskController.Bounds.size;
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
            _spline.Evaluate(currentRelativePosition, out var worldPosition, out var tangent, out var upVector);
            var domino = Instantiate(_nonPlayableDominoPrefab);
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
