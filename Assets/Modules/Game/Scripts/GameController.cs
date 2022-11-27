using System;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private IGameModel _gameModel;
    void Awake()
    {
        _gameModel.FallingCountdownFinished += OnFallingCountdownFinished;
        _gameModel.ShouldLoadNextLevel += LoadNextLevel;
        _gameModel.ShouldRestartGame += RestartGame;
    }

    private void RestartGame()
    {
        _gameModel.CurrentScore = 0;
    }

    private void LoadNextLevel()
    {
        throw new NotImplementedException();
    }

    private void OnFallingCountdownFinished()
    {
        if (_gameModel.HaveAllNonPlayableDominosFallenDown)
        {
            _gameModel.CurrentScore++;
            _gameModel.TriggerLevelSucceded();
            return;
        }

        _gameModel.TriggerLevelFailed();
    }

    private void OnDestroy()
    {
        _gameModel.FallingCountdownFinished -= OnFallingCountdownFinished;
        _gameModel.ShouldLoadNextLevel -= LoadNextLevel;
        _gameModel.ShouldRestartGame -= RestartGame;
    }
}
