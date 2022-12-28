using System;
using System.Collections.Generic;

public interface IGameModel
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
    public GameMode CurrentGameMode { get; set; }
    public IReadOnlyDictionary<IDesk, bool> HasAtLeastOneNonPlayableDominoFallen { get; }
    public IReadOnlyDictionary<IDesk, bool> HaveAllNonPlayableDominosFallenDown { get; }
    public IReadOnlyDictionary<IDesk, int> CurrentScore { get; }
    public IReadOnlyDictionary<IDesk, IList<IBowl>> Bowls { get; }
    public void TriggerLevelSucceded(IDesk desk);
    public void TriggerLevelFailed(IDesk desk);
    public void LoadNextLevel(IDesk desk);
    public void RestartGame(IDesk desk);
    public void SetHasAtLeastOneNonPlayableDominoFallen(IDesk desk, bool condition);
    public void SetIsFallingCountdownFinished(IDesk desk, bool isFinished);
    public void SetCurrentScore(IDesk desk, int score);
    public void AddBowl(IDesk desk, IBowl bowl);
    public void SetDominoFellDown(IDomino domino);
    public void AddSpawnedNonPlayableDominos(IDesk desk, List<IDomino> dominos);
    public void ClearNonPlayableDominos(IDesk desk);
    public List<IDomino> GetNonPlayableDominosForDesk(IDesk desk);
}
