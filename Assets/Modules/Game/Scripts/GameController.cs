using System;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private IGameModel _gameModel;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    void Awake()
    {
        _gameModel.FallingCountdownFinished += OnFallingCountdownFinished;
        _gameModel.ShouldLoadNextLevel += LoadNextLevel;
        _gameModel.ShouldRestartGame += RestartGame;
        _sceneSetupModel.GameStarted += OnGameStarted;
    }
    private void OnDestroy()
    {
        _gameModel.FallingCountdownFinished -= OnFallingCountdownFinished;
        _gameModel.ShouldLoadNextLevel -= LoadNextLevel;
        _gameModel.ShouldRestartGame -= RestartGame;
        _sceneSetupModel.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted(IDesk desk)
    {
        _gameModel.SetCurrentScore(desk, 0);
    }

    private void RestartGame(IDesk desk)
    {
        _gameModel.SetCurrentScore(desk, 0);
    }

    private void LoadNextLevel(IDesk desk)
    {
        //throw new NotImplementedException();
    }

    private void OnFallingCountdownFinished(IDesk desk)
    {
        if (_gameModel.HaveAllNonPlayableDominosFallenDown[desk])
        {
            _gameModel.SetCurrentScore(desk, _gameModel.CurrentScore[desk] + 1);
            _gameModel.TriggerLevelSucceded(desk);
            return;
        }

        _gameModel.TriggerLevelFailed(desk);
    }
}
