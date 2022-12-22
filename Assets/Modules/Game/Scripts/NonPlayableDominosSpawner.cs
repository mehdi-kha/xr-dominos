using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Splines;
using Zenject;

[Serializable]
public struct LevelStruct
{
    public int Number;
    public GameLevel GameLevel;
}
public class NonPlayableDominosSpawner : MonoBehaviour
{
    [SerializeField] private List<LevelStruct> _levels;
    [SerializeField] private GameObject _nonPlayableDominoPrefab;
    [SerializeField] private float _distanceBetweenDominos = 0.05f;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Inject] private IGameModel _gameModel;
    [Range(0, 1)]
    [SerializeField] private float _verticalSpawningOffset = 0.05f;

    private ObjectPool<GameObject> _nonPlayableDominosPool;
    private Dictionary<IDesk, List<GameObject>> _spawnedNonPlayableDominos = new();

    private void Awake()
    {
        _sceneSetupModel.GameStarted += OnGameStarted;
        _gameModel.ShouldLoadNextLevel += OnShouldLoadNextLevel;
        _gameModel.ShouldRestartGame += OnShouldRestartGame;

        _nonPlayableDominosPool = new ObjectPool<GameObject>(CreateDomino, OnTakeDominoFromPool, OnReleaseDominoToPool, null, true, 30);
    }

    private void OnDestroy()
    {
        _sceneSetupModel.GameStarted -= OnGameStarted;
        _gameModel.ShouldLoadNextLevel -= OnShouldLoadNextLevel;
        _gameModel.ShouldRestartGame -= OnShouldRestartGame;
    }

    private GameObject CreateDomino()
    {
        var instance = Instantiate(_nonPlayableDominoPrefab);
        instance.SetActive(false);
        return instance;
    }

    private void OnTakeDominoFromPool(GameObject domino)
    {
        domino.SetActive(true);
    }

    private void OnReleaseDominoToPool(GameObject domino)
    {
        domino.SetActive(false);
    }

    private void OnShouldRestartGame(IDesk desk)
    {
        DespawnNonPlayableDominosForDesk(desk);
        SpawnNonPlayableDominosForLevel(desk, 0);
    }

    private void OnShouldLoadNextLevel(IDesk desk)
    {
        DespawnNonPlayableDominosForDesk(desk);
        SpawnNonPlayableDominosForLevel(desk, _gameModel.CurrentScore[desk]);
    }

    private void OnGameStarted(IDesk desk)
    {
        SpawnNonPlayableDominosForLevel(desk, 0);
    }

    private void SpawnNonPlayableDominosForLevel(IDesk desk, int level)
    {
        var gameLevel = GetGameLevelForLevel(level);
        var correspondingSplineContainerPrefab = gameLevel.SplineContainer;
        var spline = SpawnSpline(desk, correspondingSplineContainerPrefab);
        var spawnedDominos = SpawnNonPlayableDominosAlongSpline(spline);
        var minToDestroy = (int) (gameLevel.MinimumPercentageToDestroy * spawnedDominos.Count);
        var maxToDestroy = (int) (gameLevel.MaxPercentageToDestroy * spawnedDominos.Count);
        RandomlyDestroyDominos(spawnedDominos, minToDestroy, maxToDestroy);
        if (!_spawnedNonPlayableDominos.ContainsKey(desk))
        {
            _spawnedNonPlayableDominos[desk] = new List<GameObject>();
        }

        _spawnedNonPlayableDominos[desk].AddRange(spawnedDominos);
    }

    private void DespawnNonPlayableDominosForDesk(IDesk desk)
    {
        var dominos = _spawnedNonPlayableDominos[desk];
        foreach (var domino in dominos)
        {
            _nonPlayableDominosPool.Release(domino);
        }
        _spawnedNonPlayableDominos[desk].Clear();
    }

    private GameLevel GetGameLevelForLevel(int searchedLevel)
    {
        LevelStruct foundLevel;
        try
        {
            foundLevel = _levels.First(level => level.Number == searchedLevel);
        }

        catch (InvalidOperationException e)
        {
            throw new NullReferenceException("No GameLevel for the level 0 was registered.");
        }

        return foundLevel.GameLevel;
    }

    private SplineContainer SpawnSpline(IDesk deskController, SplineContainer splineContainerPrefab)
    {
        // Spawn spline on top of the desk
        var spline = Instantiate(splineContainerPrefab, deskController.Transform);
        // Position the spline on top of the desk
        spline.transform.position += deskController.CenterTopPosition - deskController.Transform.position;
        // Add offset
        spline.transform.localPosition += _verticalSpawningOffset * spline.transform.up;
        // Adapt the spline to match the table
        spline.transform.localScale = deskController.Bounds.size;

        return spline;
    }

    private List<GameObject> SpawnNonPlayableDominosAlongSpline(SplineContainer spline)
    {
        var spawnedDominos = new List<GameObject>();
        float currentRelativePosition = 0;
        var splineLength = spline.CalculateLength();
        var relativeDistanceBetweenDominos = _distanceBetweenDominos / splineLength;
        while (currentRelativePosition < 1)
        {
            spline.Evaluate(currentRelativePosition, out var worldPosition, out var tangent, out var upVector);
            var domino = _nonPlayableDominosPool.Get();
            spawnedDominos.Add(domino);
            domino.transform.position = worldPosition;
            domino.transform.LookAt(worldPosition + tangent, upVector);
            currentRelativePosition += relativeDistanceBetweenDominos;
        }

        return spawnedDominos;
    }

    private void RandomlyDestroyDominos(List<GameObject> dominos, int minToDestroy, int maxToDestroy)
    {
        int numToDestroy = UnityEngine.Random.Range(minToDestroy, maxToDestroy + 1);

        for (int i = 0; i < numToDestroy; i++)
        {
            int indexToRemove = UnityEngine.Random.Range(0, dominos.Count);
            Destroy(dominos[indexToRemove]);
            dominos.RemoveAt(indexToRemove);
        }
    }
}
