using Oculus.Interaction;
using UnityEngine;
using Zenject;

public class StartGameMenuController : MenuController
{
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _startGameButton;
    private void Awake()
    {
        _sceneSetupModel.DeskDetected += OnDeskSpawned;
        _startGameButton.WhenPointerEventRaised += OnStartGameButtonPointerEvent;
        _sceneSetupModel.GameStarted += OnGameStarted;
        _sceneSetupModel.UserFootprintsStatusChanged += OnUserFootprintStatusChanged;
        HideMenu();
    }

    private void OnDestroy()
    {
        _sceneSetupModel.DeskDetected -= OnDeskSpawned;
        _startGameButton.WhenPointerEventRaised -= OnStartGameButtonPointerEvent;
        _sceneSetupModel.GameStarted -= OnGameStarted;
        _sceneSetupModel.UserFootprintsStatusChanged -= OnUserFootprintStatusChanged;
    }

    private void OnUserFootprintStatusChanged(bool isUserOnFootprints)
    {
        if (!_sceneSetupModel.HaveDesksBeenDetected)
        {
            _visuals.SetActive(false);
            return;
        }

        if (!isUserOnFootprints)
        {
            HideMenu();
            return;
        }

        ShowMenu();
    }

    private void OnGameStarted()
    {
        HideMenu();
    }

    private void OnStartGameButtonPointerEvent(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }

        _sceneSetupModel.RaiseGameStarted();
    }

    private void OnDeskSpawned()
    {
        if (!_sceneSetupModel.HaveDesksBeenDetected)
        {
            _visuals.SetActive(false);
            return;
        }

        ShowMenu();
    }
}
