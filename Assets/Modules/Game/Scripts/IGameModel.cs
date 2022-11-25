using System;

public interface IGameModel
{
    public event Action<GameMode> GameModeChanged;
    public event Action FirstNonPlayableDominoFell;
    public GameMode CurrentGameMode { get; set; }
    public bool HasAtLeastOneNonPlayableDominoFallen { get; set; }
}
