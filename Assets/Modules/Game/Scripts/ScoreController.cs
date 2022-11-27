using Oculus.Interaction;
using System;
using UnityEngine;
using Zenject;

public class ScoreController : MenuController
{
    [Inject] private IGameModel _gameModel;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _restartButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _continueButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _quitGameButton;

    protected void Awake()
    {
        _gameModel.FallingCountdownFinished += OnFallingCountdownFinished;
        _quitGameButton.WhenPointerEventRaised += OnQuitGameButtonPointerRaised;
        _continueButton.WhenPointerEventRaised += OnContinueButtonPointerRaised;
        _restartButton.WhenPointerEventRaised += OnRestartButtonPointerRaised;
        HideMenu();
    }

    private void OnContinueButtonPointerRaised(PointerEvent obj)
    {
        throw new NotImplementedException();
    }

    private void OnRestartButtonPointerRaised(PointerEvent obj)
    {
        throw new NotImplementedException();
    }

    private void OnDestroy()
    {
        _gameModel.FallingCountdownFinished -= OnFallingCountdownFinished;
        _quitGameButton.WhenPointerEventRaised -= OnQuitGameButtonPointerRaised;
        _continueButton.WhenPointerEventRaised -= OnContinueButtonPointerRaised;
        _restartButton.WhenPointerEventRaised -= OnRestartButtonPointerRaised;
    }

    private void OnQuitGameButtonPointerRaised(PointerEvent obj)
    {
        _sceneSetupModel.CloseApp();
    }

    private void OnFallingCountdownFinished()
    {
        if (_gameModel.HaveAllNonPlayableDominosFallenDown)
        {
            ShowSuccessPanel();
            return;
        }

        ShowFailurePanel();
    }

    private void ShowSuccessPanel()
    {
        // TODO override the ShowMenu method to make the panel appear on top of the desk for which all dominos have fallen
        ShowMenu();
        _continueButton.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(false);
    }

    private void ShowFailurePanel()
    {
        ShowMenu();
        _continueButton.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(true);
    }
}
