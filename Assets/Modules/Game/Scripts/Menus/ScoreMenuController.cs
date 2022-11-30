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

    private IDesk _currentDesk;

    protected void Awake()
    {
        _gameModel.LevelSucceeded += OnLevelSucceded;
        _gameModel.LevelFailed += OnLevelFailed;
        _quitGameButton.WhenPointerEventRaised += OnQuitGameButtonPointerRaised;
        _continueButton.WhenPointerEventRaised += OnContinueButtonPointerRaised;
        _restartButton.WhenPointerEventRaised += OnRestartButtonPointerRaised;
        HideMenu();
    }

    private void OnLevelFailed(IDesk desk)
    {
        _currentDesk = desk;
        ShowFailurePanel();
    }

    private void OnLevelSucceded(IDesk desk)
    {
        _currentDesk = desk;
        ShowSuccessPanel();
    }

    private void SetScoreText(int newScore)
    {
        _scoreText.text = $"{_scorePrefixText}{newScore}";
    }

    private void OnContinueButtonPointerRaised(PointerEvent obj)
    {
        _gameModel.LoadNextLevel(_currentDesk);
        HideMenu();
    }

    private void OnRestartButtonPointerRaised(PointerEvent obj)
    {
        _gameModel.RestartGame(_currentDesk);
        HideMenu();
    }

    private void OnDestroy()
    {
        _quitGameButton.WhenPointerEventRaised -= OnQuitGameButtonPointerRaised;
        _continueButton.WhenPointerEventRaised -= OnContinueButtonPointerRaised;
        _restartButton.WhenPointerEventRaised -= OnRestartButtonPointerRaised;
    }

    private void OnQuitGameButtonPointerRaised(PointerEvent obj)
    {
        _sceneSetupModel.CloseApp();
    }

    private void ShowSuccessPanel()
    {
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

    protected override void ShowMenu()
    {
        base.ShowMenu();
        // TODO make the panel appear on top of the desk for which all dominos have fallen? Explore the idea

        SetScoreText(_gameModel.CurrentScore[_currentDesk]);
    }
}
