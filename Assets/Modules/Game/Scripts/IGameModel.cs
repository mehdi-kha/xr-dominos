using System;

public interface IGameModel
{
    public event Action<GameMode> GameModeChanged;
    public GameMode CurrentGameMode { get; set; }
}
