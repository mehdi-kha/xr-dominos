using System;
using System.Collections.Generic;

public class GameModel : IGameModel
{
    private XRMode _currentGameMode;
    private Dictionary<IDesk, bool> _hasFirstNonPlayableDominoFallen = new();
    private Dictionary<IDesk, bool> _isFallingCountdownFinished = new();
    private Dictionary<IDesk, bool> _haveAllNonPlayableDominosFallenDown = new();
    private Dictionary<IDesk, int> _currentScore = new();
    private Dictionary<IDesk, IList<IBowl>> _bowls = new();
    public XRMode CurrentXRMode
    {
        get => _currentGameMode;
        set 
        {
            _currentGameMode = value;
            XRModeChanged?.Invoke(value);
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

    public IReadOnlyDictionary<IDesk, bool> IsFallingCountdownFinished => _isFallingCountdownFinished;

    public void SetIsFallingCOuntdownFinished(IDesk desk, bool isFinished)
    {
        if (isFinished)
        {
            FallingCountdownFinished?.Invoke(desk);
        }
        _isFallingCountdownFinished[desk] = isFinished;
    }

    public IReadOnlyDictionary<IDesk, bool> HaveAllNonPlayableDominosFallenDown => _haveAllNonPlayableDominosFallenDown;

    public void SetHaveAllNonPlayableDominosFallen(IDesk desk, bool haveAllFallen)
    {
        if (haveAllFallen)
        {
            AllNonPlayableDominosFell?.Invoke(desk);
        }

        _haveAllNonPlayableDominosFallenDown[desk] = haveAllFallen;
    }

    public IReadOnlyDictionary<IDesk, int> CurrentScore => _currentScore;

    public IReadOnlyDictionary<IDesk, IList<IBowl>> Bowls => _bowls;

    public void SetCurrentScore(IDesk desk, int score)
    {
        _currentScore[desk] = score;
    }

    public event Action<XRMode> XRModeChanged;
    public event Action<IDesk> FirstNonPlayableDominoFell;
    public event Action<IDesk> AllNonPlayableDominosFell;
    public event Action<IDesk> FallingCountdownFinished;
    public event Action<IDesk> LevelSucceeded;
    public event Action<IDesk> LevelFailed;
    public event Action<IDesk> ShouldLoadNextLevel;
    public event Action<IDesk> ShouldRestartGame;

    private void ResetDataForDesk(IDesk desk)
    {
        SetHasAtLeastOneNonPlayableDominoFallen(desk, false);
        SetIsFallingCOuntdownFinished(desk, false);
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
}
