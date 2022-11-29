using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class BowlController : MonoBehaviour
{
    public IDesk CorrespondingDesk;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Inject] private IGameModel _gameModel;
    [SerializeField] private GameObject _visuals;
    [SerializeField] private DominoController _dominoPrefab;
    [SerializeField] private string _dominoTag = "Domino";
    [SerializeField] private Transform _dominoSpawningSpot;
    [Tooltip("If true, some horizontal vector with a random direction will be applied to the spawning spot for each domino being spawned.")]
    [SerializeField] private bool _addRandomnessAroundTheSpawningSpot = true;
    [Tooltip("Only relevant is spawning randomness is enabled. Max distance from the defined spawning spot.")]
    [SerializeField] private float _spawningRandomnessRadius = 0.05f;
    [SerializeField] private int _numberOfDominosToSpawn = 10;
    [SerializeField] private BowlLeavingDominosController _bowlLeavingDominosController;

    private ObjectPool<DominoController> _dominoPool;
    private List<DominoController> _spawnedDominos = new List<DominoController>();

    private void Awake()
    {
        _dominoPool = new ObjectPool<DominoController>(CreateDomino, OnTakeDominoFromPool, OnReleaseToPool, null, true, 20);
        _sceneSetupModel.GameStarted += OnGameStarted;
        _gameModel.FirstNonPlayableDominoFell += OnFirstNonPlayableDominoFell;
        _gameModel.ShouldLoadNextLevel += OnShouldLoadNextLevel;
        _gameModel.ShouldRestartGame += OnShouldRestartGame;
    }

    private void OnDestroy()
    {
        _sceneSetupModel.GameStarted -= OnGameStarted;
        _gameModel.FirstNonPlayableDominoFell -= OnFirstNonPlayableDominoFell;
        _gameModel.ShouldLoadNextLevel -= OnShouldLoadNextLevel;
        _gameModel.ShouldRestartGame -= OnShouldRestartGame;
    }

    private void OnShouldRestartGame(IDesk desk)
    {
        if (desk != CorrespondingDesk)
        {
            return;
        }

        DespawnDominos();
        PopulateBowl();
    }

    private void OnShouldLoadNextLevel(IDesk desk)
    {
        if (desk != CorrespondingDesk)
        {
            return;
        }

        DespawnDominos();
        PopulateBowl();
    }

    private void Start()
    {
        if (!_sceneSetupModel.HasGameStarted.ContainsKey(CorrespondingDesk) || !_sceneSetupModel.HasGameStarted[CorrespondingDesk])
        {
            Hide();
        }
    }

    private void OnFirstNonPlayableDominoFell(IDesk desk)
    {
        if (desk != CorrespondingDesk)
        {
            return;
        }

        MakeAllPlayableDominosNonInteractable();
        DissolveDominosInBowl();
    }

    private void MakeAllPlayableDominosNonInteractable()
    {
        foreach (var domino in _spawnedDominos)
        {
            domino.MakeNonInteractable();
        }
    }

    private void DissolveDominosInBowl()
    {
        foreach (var domino in _bowlLeavingDominosController.DominosInBowl)
        {
            domino.Value.Hide();
        }
    }

    private void PopulateBowl()
    {
        for (int i = 0; i < _numberOfDominosToSpawn; i++)
        {
            var domino = _dominoPool.Get();
            domino.Show();
            domino.MakeInteractable();
            _spawnedDominos.Add(domino);
        }
    }

    private void DespawnDominos()
    {
        foreach (var domino in _spawnedDominos)
        {
            _dominoPool.Release(domino);
        }

        _spawnedDominos.Clear();
    }

    private DominoController CreateDomino()
    {
        return Instantiate(_dominoPrefab);
    }

    private void OnTakeDominoFromPool(DominoController domino)
    {
        domino.gameObject.SetActive(true);
        var dominoPosition = _dominoSpawningSpot.position;
        if (_addRandomnessAroundTheSpawningSpot)
        {
            dominoPosition = RandomizeSpawningPosition(dominoPosition);
        }
        domino.transform.position = dominoPosition;
    }

    private void OnReleaseToPool(DominoController domino)
    {
        domino.gameObject.SetActive(false);
    }

    private Vector3 RandomizeSpawningPosition(Vector3 spawningPosition)
    {
        var randomRotation = UnityEngine.Random.Range(0, 360);
        var randomVector = Quaternion.AngleAxis(randomRotation, Vector3.up) * Vector3.right;
        spawningPosition += UnityEngine.Random.Range(0, 0.05f) * randomVector;
        return spawningPosition;
    }

    private void OnGameStarted(IDesk desk)
    {
        if (desk != CorrespondingDesk)
        {
            return;
        }

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
