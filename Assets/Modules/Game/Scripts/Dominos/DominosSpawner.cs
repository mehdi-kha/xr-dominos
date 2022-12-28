using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class DominosSpawner : MonoBehaviour
{
    [Inject] private IGameModel _gameModel;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Inject] private DominoFactory _dominosFactory;

    private ObjectPool<IDomino> _dominoPool;
    private Dictionary<IBowl, IList<IDomino>> _spawnedDominos = new ();
    private List<IDomino> _dominosInHand = new();

    private void Awake()
    {
        _dominoPool = new ObjectPool<IDomino>(CreateDomino, OnTakeDominoFromPool, OnReleaseToPool, OnDominoDestroyed, true, 20);
        _gameModel.FirstNonPlayableDominoFell += OnFirstNonPlayableDominoFell;
        _gameModel.ShouldLoadNextLevel += SpawnDominosForDesk;
        _gameModel.ShouldRestartGame += OnShouldRestartGame;
        _sceneSetupModel.GameStarted += SpawnDominosForDesk;
        _gameModel.LevelSucceeded += OnLevelSucceeded;
        _gameModel.LevelFailed += OnLevelFailed;
    }

    private void OnDestroy()
    {
        _gameModel.FirstNonPlayableDominoFell -= OnFirstNonPlayableDominoFell;
        _gameModel.ShouldLoadNextLevel -= SpawnDominosForDesk;
        _gameModel.ShouldRestartGame -= OnShouldRestartGame;
        _sceneSetupModel.GameStarted -= SpawnDominosForDesk;
        _gameModel.LevelSucceeded -= OnLevelSucceeded;
        _gameModel.LevelFailed -= OnLevelFailed;
    }

    private void OnLevelSucceeded(IDesk desk)
    {
        DissolveDominosInBowl(desk.Bowl);
    }

    private void OnLevelFailed(IDesk desk)
    {
        DissolveDominosInBowl(desk.Bowl);
    }

    private void OnDominoDestroyed(IDomino dominoController)
    {
        dominoController.OnGrabbed -= OnDominoGrabbed;
        dominoController.OnReleased -= OnDominoReleased;
    }

    private void OnShouldRestartGame(IDesk desk)
    {
        var bowls = _gameModel.Bowls[desk];
        foreach (var bowl in bowls)
        {
            DespawnDominos(bowl);
            PopulateBowl(bowl);
        }
    }

    private void SpawnDominosForDesk(IDesk desk)
    {
        var bowls = _gameModel.Bowls[desk];
        foreach (var bowl in bowls)
        {
            DespawnDominos(bowl);
            PopulateBowl(bowl);
        }
    }

    private void OnFirstNonPlayableDominoFell(IDesk desk)
    {
        var bowls = _gameModel.Bowls[desk];
        foreach (var bowl in bowls)
        {
            MakeDominosNonInteractable(_spawnedDominos[bowl]);
        }

        DissolveDominosInHand();
    }

    private void MakeDominosNonInteractable(IList<IDomino> dominos)
    {
        foreach (var domino in dominos)
        {
            domino.MakeNonInteractable();
        }
    }

    private void DissolveDominosInBowl(IBowl bowl)
    {
        foreach (var domino in _spawnedDominos[bowl])
        {
            domino.Hide();
        }
    }

    private void DissolveDominosInHand()
    {
        foreach (var domino in _dominosInHand)
        {
            domino.Hide();
        }
    }

    private void PopulateBowl(IBowl bowl)
    {
        for (int i = 0; i < bowl.NumberOfDominosToSpawn; i++)
        {
            var domino = _dominoPool.Get();
            domino.SetBowl(bowl);
            domino.SetPosition(bowl.GetDominoSpawningPosition());
            domino.Show();
            domino.MakeInteractable();
            if (_spawnedDominos[bowl] == null)
            {
                _spawnedDominos[bowl] = new List<IDomino>();
            }
            _spawnedDominos[bowl].Add(domino);
        }
    }

    private void DespawnDominos(IBowl bowl)
    {
        IList<IDomino> spawnedDominosForBowl;
        if (!_spawnedDominos.TryGetValue(bowl, out spawnedDominosForBowl))
        {
            spawnedDominosForBowl = new List<IDomino>();
            _spawnedDominos[bowl] = spawnedDominosForBowl;
        }

        foreach (var domino in spawnedDominosForBowl)
        {
            _dominoPool.Release(domino);
        }

        spawnedDominosForBowl.Clear();
    }

    private IDomino CreateDomino()
    {
        var dominoController = _dominosFactory.Create();
        dominoController.OnGrabbed += OnDominoGrabbed;
        dominoController.OnReleased += OnDominoReleased;
        dominoController.GameModel = _gameModel;
        return dominoController;
    }

    private void OnDominoGrabbed(IDomino dominoController)
    {
        _dominosInHand.Add(dominoController);
    }

    private void OnDominoReleased(IDomino dominoController)
    {
        _dominosInHand.Remove(dominoController);
    }

    private void OnTakeDominoFromPool(IDomino domino)
    {
        domino.SetActive(true);
    }

    private void OnReleaseToPool(IDomino domino)
    {
        domino.SetActive(false);
    }
}
