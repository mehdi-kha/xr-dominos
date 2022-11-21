using System;

public class GameModel : IGameModel
{
    private GameMode _currentGameMode;
    public GameMode CurrentGameMode
    {
        get => _currentGameMode;
        set 
        {
            _currentGameMode = value;
            GameModeChanged?.Invoke(value);
        }
    }

    public event Action<GameMode> GameModeChanged;
}
