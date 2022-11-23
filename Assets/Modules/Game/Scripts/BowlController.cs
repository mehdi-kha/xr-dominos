using Oculus.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class BowlController : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [SerializeField] private GameObject _visuals;
    [SerializeField] private GameObject _dominoPrefab;
    [SerializeField] private string _dominoTag = "Domino";
    [SerializeField] private Transform _dominoSpawningSpot;
    [Tooltip("If true, some horizontal vector with a random direction will be applied to the spawning spot for each domino being spawned.")]
    [SerializeField] private bool _addRandomnessAroundTheSpawningSpot = true;
    [Tooltip("Only relevant is spawning randomness is enabled. Max distance from the defined spawning spot.")]
    [SerializeField] private float _spawningRandomnessRadius = 0.05f;
    [SerializeField] private int _numberOfDominosToSpawn = 10;

    private ObjectPool<GameObject> _dominoPool;

    private void Awake()
    {
        _dominoPool = new ObjectPool<GameObject>(CreateDomino, OnTakeDominoFromPool, null, null, true, 20);
        if (!_sceneSetupModel.HasGameStarted)
        {
            Hide();
        }

        _sceneSetupModel.GameStarted += OnGameStarted;
    }

    private void PopulateBowl()
    {
        for(int i = 0; i < _numberOfDominosToSpawn; i++)
        {
            _dominoPool.Get();
        }
    }

    private GameObject CreateDomino()
    {
        return Instantiate(_dominoPrefab);
    }

    private void OnTakeDominoFromPool(GameObject domino)
    {
        var dominoPosition = _dominoSpawningSpot.position;
        if (_addRandomnessAroundTheSpawningSpot)
        {
            dominoPosition = RandomizeSpawningPosition(dominoPosition);
        }
        domino.transform.position = dominoPosition;
    }

    private Vector3 RandomizeSpawningPosition(Vector3 spawningPosition)
    {
        var randomRotation = UnityEngine.Random.Range(0, 360);
        var randomVector = Quaternion.AngleAxis(randomRotation, Vector3.up) * Vector3.right;
        spawningPosition += UnityEngine.Random.Range(0, 0.05f) * randomVector;
        return spawningPosition;
    }

    private void OnGameStarted()
    {
        ShowAndPopulateWithDominos();
    }

    private void Hide()
    {
        //TODO improve this with animations
        _visuals.SetActive(false);
    }

    private void ShowAndPopulateWithDominos()
    {
        _visuals.SetActive(true);
        PopulateBowl();
    }
}
