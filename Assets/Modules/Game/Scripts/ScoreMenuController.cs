using Oculus.Interaction;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreMenuController : MenuController
{
    [Inject] private IGameModel _gameModel;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _restartButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _continueButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _quitGameButton;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private string _scorePrefixText = "Current score: ";

    protected void Awake()
    {
        _gameModel.LevelSucceeded += OnLevelSucceded;
        _gameModel.LevelFailed += OnLevelFailed;
        _quitGameButton.WhenPointerEventRaised += OnQuitGameButtonPointerRaised;
        _continueButton.WhenPointerEventRaised += OnContinueButtonPointerRaised;
        _restartButton.WhenPointerEventRaised += OnRestartButtonPointerRaised;
        _gameModel.CurrentScoreChanged += SetScoreText;
        SetScoreText(_gameModel.CurrentScore);
        HideMenu();
    }

    private void OnLevelFailed()
    {
        ShowFailurePanel();
    }

    private void OnLevelSucceded()
    {
        ShowSuccessPanel();
    }

    private void SetScoreText(int newScore)
    {
        _scoreText.text = $"{_scorePrefixText}{newScore}";
    }

    private void OnContinueButtonPointerRaised(PointerEvent obj)
    {
        _gameModel.LoadNextLevel();
        HideMenu();
    }

    private void OnRestartButtonPointerRaised(PointerEvent obj)
    {
        _gameModel.RestartGame();
        HideMenu();
    }

    private void OnDestroy()
    {
        _quitGameButton.WhenPointerEventRaised -= OnQuitGameButtonPointerRaised;
        _continueButton.WhenPointerEventRaised -= OnContinueButtonPointerRaised;
        _restartButton.WhenPointerEventRaised -= OnRestartButtonPointerRaised;
        _gameModel.CurrentScoreChanged -= SetScoreText;
    }

    private void OnQuitGameButtonPointerRaised(PointerEvent obj)
    {
        _sceneSetupModel.CloseApp();
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
