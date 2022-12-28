using System;
using System.Collections.Generic;

public class GameModel : IGameModel
{
    public event Action<GameMode> GameModeChanged;
    public event Action<IDesk> FirstNonPlayableDominoFell;
    public event Action<IDesk> AllNonPlayableDominosFell;
    public event Action<IDesk> FallingCountdownFinished;
    public event Action<IDesk> LevelSucceeded;
    public event Action<IDesk> LevelFailed;
    public event Action<IDesk> ShouldLoadNextLevel;
    public event Action<IDesk> ShouldRestartGame;
    public event Action<IDomino> DominoFellDown;

    private GameMode _currentGameMode;
    private Dictionary<IDesk, bool> _hasFirstNonPlayableDominoFallen = new();
    private Dictionary<IDesk, bool> _isFallingCountdownFinished = new();
    private Dictionary<IDesk, bool> _haveAllNonPlayableDominosFallenDown = new();
    private Dictionary<IDesk, int> _currentScore = new();
    private Dictionary<IDesk, IList<IBowl>> _bowls = new();
    private Dictionary<IDesk, List<IDomino>> _spawnedNonPlayableDominos = new();

    public GameMode CurrentGameMode
    {
        get => _currentGameMode;
        set 
        {
            _currentGameMode = value;
            GameModeChanged?.Invoke(value);
        }
    }

    public IReadOnlyDictionary<IDesk, bool> HasAtLeastOneNonPlayableDominoFallen => _hasFirstNonPlayableDominoFallen;

    public void SetHasAtLeastOneNonPlayableDominoFallen(IDesk desk, bool condition)
    {
        if ((!_hasFirstNonPlayableDominoFallen.ContainsKey(desk) || !_hasFirstNonPlayableDominoFallen[desk]) && condition)
        {
            FirstNonPlayableDominoFell?.Invoke(desk);
        }

        _hasFirstNonPlayableDominoFallen[desk] = condition;
    }

    public void SetIsFallingCountdownFinished(IDesk desk, bool isFinished)
    {
        if (!isFinished)
        {
            return;
        }

        // Check the dominos status, and set the right status
        SetHaveAllNonPlayableDominosFallen(desk, CheckDominosFallenStatus(desk));

        FallingCountdownFinished?.Invoke(desk);
        _isFallingCountdownFinished[desk] = isFinished;
    }

    public IReadOnlyDictionary<IDesk, bool> HaveAllNonPlayableDominosFallenDown => _haveAllNonPlayableDominosFallenDown;

    private bool CheckDominosFallenStatus(IDesk desk)
    {
        var dominos = _spawnedNonPlayableDominos[desk];
        return dominos.TrueForAll(domino => domino.HasFallenDown);
    }

    private void SetHaveAllNonPlayableDominosFallen(IDesk desk, bool haveAllFallen)
    {
        if (haveAllFallen)
        {
            AllNonPlayableDominosFell?.Invoke(desk);
        }

        _haveAllNonPlayableDominosFallenDown[desk] = haveAllFallen;
    }

    public IReadOnlyDictionary<IDesk, int> CurrentScore => _currentScore;

    public IReadOnlyDictionary<IDesk, IList<IBowl>> Bowls => _bowls;

    public List<IDomino> GetNonPlayableDominosForDesk(IDesk desk)
    {
        return _spawnedNonPlayableDominos[desk];
    }

    public void AddSpawnedNonPlayableDominos(IDesk desk, List<IDomino> dominos)
    {
        if (!_spawnedNonPlayableDominos.ContainsKey(desk))
        {
            _spawnedNonPlayableDominos[desk] = new();
        }
        
        _spawnedNonPlayableDominos[desk].AddRange(dominos);
    }

    public void SetCurrentScore(IDesk desk, int score)
    {
        _currentScore[desk] = score;
    }

    private void ResetDataForDesk(IDesk desk)
    {
        SetHasAtLeastOneNonPlayableDominoFallen(desk, false);
        SetIsFallingCountdownFinished(desk, false);
        SetHaveAllNonPlayableDominosFallen(desk, false);
    }

    public void LoadNextLevel(IDesk desk)
    {
        ResetDataForDesk(desk);
        ShouldLoadNextLevel?.Invoke(desk);
    }

    public void RestartGame(IDesk desk)
    {
        ResetDataForDesk(desk);
        ShouldRestartGame?.Invoke(desk);
    }

    public void TriggerLevelFailed(IDesk desk)
    {
        LevelFailed?.Invoke(desk);
    }

    public void TriggerLevelSucceded(IDesk desk)
    {
        LevelSucceeded?.Invoke(desk);
    }

    public void AddBowl(IDesk desk, IBowl bowl)
    {
        if (desk == null || bowl == null)
        {
            throw new NullReferenceException("Bowl or desk is null");
        }

        IList<IBowl> bowlsForDesk;
        if (!_bowls.TryGetValue(desk, out bowlsForDesk))
        {
            bowlsForDesk = new List<IBowl>();
        }

        bowlsForDesk.Add(bowl);
        _bowls[desk] = bowlsForDesk;
    }

    public void SetDominoFellDown(IDomino domino)
    {
        DominoFellDown?.Invoke(domino);
    }

    public void ClearNonPlayableDominos(IDesk desk)
    {
        _spawnedNonPlayableDominos[desk].Clear();
    }
}
