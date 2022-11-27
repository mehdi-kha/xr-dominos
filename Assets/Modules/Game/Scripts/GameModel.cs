using System;

public class GameModel : IGameModel
{
    private GameMode _currentGameMode;
    private bool _hasFirstNonPlayableDominoFallen;
    private bool _isFallingCountdownFinished;
    private bool _haveAllNonPlayableDominosFallenDown;
    private int _currentScore;
    public GameMode CurrentGameMode
    {
        get => _currentGameMode;
        set 
        {
            _currentGameMode = value;
            GameModeChanged?.Invoke(value);
        }
    }

    public bool HasAtLeastOneNonPlayableDominoFallen
    {
        get => _hasFirstNonPlayableDominoFallen;
        set
        {
            if (!_hasFirstNonPlayableDominoFallen)
            {
                FirstNonPlayableDominoFell?.Invoke();
            }

            _hasFirstNonPlayableDominoFallen = value;
        }
    }

    public bool IsFallingCountdownFinished 
    { 
        get => _isFallingCountdownFinished;
        set
        {
            if (value)
            {
                FallingCountdownFinished?.Invoke();
            }

            _isFallingCountdownFinished = value;
        }
    }

    public bool HaveAllNonPlayableDominosFallenDown
    { 
        get => _haveAllNonPlayableDominosFallenDown; 
        set
        {
            if (value)
            {
                AllNonPlayableDominosFell?.Invoke();
            }

            _haveAllNonPlayableDominosFallenDown = value;
        }
    }

    public int CurrentScore
    { 
        get => _currentScore;
        set
        {
            CurrentScoreChanged?.Invoke(value);
            _currentScore = value;
        }
    }

    public event Action<GameMode> GameModeChanged;
    public event Action FirstNonPlayableDominoFell;
    public event Action AllNonPlayableDominosFell;
    public event Action FallingCountdownFinished;
    public event Action<int> CurrentScoreChanged;
    public event Action LevelSucceeded;
    public event Action LevelFailed;
    public event Action ShouldLoadNextLevel;
    public event Action ShouldRestartGame;

    public void GoToNextLevel()
    {
        HasAtLeastOneNonPlayableDominoFallen = false;
        IsFallingCountdownFinished = false;
        HaveAllNonPlayableDominosFallenDown = false;
    }

    public void LoadNextLevel()
    {
        ShouldLoadNextLevel?.Invoke();
    }

    public void RestartGame()
    {
        ShouldRestartGame?.Invoke();
    }

    public void TriggerLevelFailed()
    {
        LevelFailed?.Invoke();
    }

    public void TriggerLevelSucceded()
    {
        LevelSucceeded?.Invoke();
    }
}
