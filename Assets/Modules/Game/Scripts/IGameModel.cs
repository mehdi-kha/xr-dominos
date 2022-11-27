using System;

public interface IGameModel
{
    public event Action<GameMode> GameModeChanged;
    public event Action FirstNonPlayableDominoFell;
    public event Action AllNonPlayableDominosFell;
    public event Action FallingCountdownFinished;
    public GameMode CurrentGameMode { get; set; }
    public bool HasAtLeastOneNonPlayableDominoFallen { get; set; }
    public bool HaveAllNonPlayableDominosFallenDown { get; set; }
    public bool IsFallingCountdownFinished { get; set; }
}
