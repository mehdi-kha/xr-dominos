using System;

public interface IGameModel
{
    public event Action<GameMode> GameModeChanged;
    public event Action FirstNonPlayableDominoFell;
    public event Action AllNonPlayableDominosFell;
    public event Action FallingCountdownFinished;
    public event Action<int> CurrentScoreChanged;
    public event Action LevelSucceeded;
    public event Action LevelFailed;
    public event Action ShouldLoadNextLevel;
    public event Action ShouldRestartGame;
    public GameMode CurrentGameMode { get; set; }
    public bool HasAtLeastOneNonPlayableDominoFallen { get; set; }
    public bool HaveAllNonPlayableDominosFallenDown { get; set; }
    public bool IsFallingCountdownFinished { get; set; }
    public int CurrentScore { get; set; }
    public void GoToNextLevel();
    public void TriggerLevelSucceded();
    public void TriggerLevelFailed();
    public void LoadNextLevel();
    public void RestartGame();
}
