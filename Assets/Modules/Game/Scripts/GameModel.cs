using System;

public class GameModel : IGameModel
{
    private GameMode _currentGameMode;
    private bool _hasFirstNonPlayableDominoFallen;
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

    public event Action<GameMode> GameModeChanged;
    public event Action FirstNonPlayableDominoFell;
}
