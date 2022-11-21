using Oculus.Interaction;
using TMPro;
using UnityEngine;
using Zenject;

public class ModeMenuController : MenuController
{
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _editModeButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _playModeButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _quitGameButton;
    [SerializeField] private TextMeshProUGUI _currentModeText;
    [SerializeField] private string _editModeText = "Current mode: <br>Edit Mode";
    [SerializeField] private string _playModeText = "Current mode: <br>Play Mode";
    [Inject] private IGameModel _gameModel;
    void Awake()
    {
        HideMenu();
        _sceneSetupModel.GameStarted += OnGameStarted;
        _editModeButton.WhenPointerEventRaised += OnEditModeButtonPointerRaised;
        _playModeButton.WhenPointerEventRaised += OnPlayModeButtonPointerRaised;
        _quitGameButton.WhenPointerEventRaised += OnQuitGameButtonPointerRaised;
        _gameModel.GameModeChanged += OnGameModeChanged;
    }

    private void OnQuitGameButtonPointerRaised(PointerEvent obj)
    {
        _sceneSetupModel.CloseApp();
    }

    private void OnGameModeChanged(GameMode currentGameMode)
    {
        UpdateMenuText(currentGameMode);
    }

    private void UpdateMenuText(GameMode currentGameMode)
    {
        _currentModeText.text = currentGameMode == GameMode.EditMode ? _editModeText : _playModeText;
    }

    private void OnPlayModeButtonPointerRaised(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }
        _gameModel.CurrentGameMode = GameMode.PlayMode;
    }

    private void OnEditModeButtonPointerRaised(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }
        _gameModel.CurrentGameMode = GameMode.EditMode;
    }

    private void OnGameStarted()
    {
        ShowMenu();
    }

    protected override void ShowMenu()
    {
        base.ShowMenu();
        UpdateMenuText(_gameModel.CurrentGameMode);
    }

    private void OnDestroy()
    {
        _sceneSetupModel.GameStarted -= OnGameStarted;
        _editModeButton.WhenPointerEventRaised -= OnEditModeButtonPointerRaised;
        _playModeButton.WhenPointerEventRaised -= OnPlayModeButtonPointerRaised;
        _quitGameButton.WhenPointerEventRaised -= OnQuitGameButtonPointerRaised;
        _gameModel.GameModeChanged -= OnGameModeChanged;
    }
}
