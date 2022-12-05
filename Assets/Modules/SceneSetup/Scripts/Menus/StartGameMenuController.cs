using Oculus.Interaction;
using UnityEngine;
using Zenject;

public class StartGameMenuController : MenuController
{
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _startGameButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _startTutorialButton;
    private void Awake()
    {
        _sceneSetupModel.DeskDetected += OnDeskDetected;
        _startGameButton.WhenPointerEventRaised += OnStartGameButtonPointerEvent;
        _startTutorialButton.WhenPointerEventRaised += OnTutorialButtonPointerEvent;
        _sceneSetupModel.GameStarted += HideMenu;
        _sceneSetupModel.TutorialStarted += HideMenu;
        _sceneSetupModel.UserFootprintsStatusChanged += OnUserFootprintStatusChanged;
        if (!_sceneSetupModel.IsUserOnFootsteps)
        {
            HideMenu();
        }
    }

    private void OnDestroy()
    {
        _sceneSetupModel.DeskDetected -= OnDeskDetected;
        _startGameButton.WhenPointerEventRaised -= OnStartGameButtonPointerEvent;
        _startTutorialButton.WhenPointerEventRaised -= OnTutorialButtonPointerEvent;
        _sceneSetupModel.GameStarted -= HideMenu;
        _sceneSetupModel.TutorialStarted -= HideMenu;
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

    private void HideMenu(IDesk desk)
    {
        HideMenu();
    }

    private void OnStartGameButtonPointerEvent(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }

        _sceneSetupModel.StartGameForAllDesks();
    }

    private void OnTutorialButtonPointerEvent(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }

        _sceneSetupModel.StartTutorialForAllDesks();
    }

    private void OnDeskDetected(DeskController deskController)
    {
        if (!_sceneSetupModel.IsUserOnFootsteps)
        {
            HideMenu();
            return;
        }

        ShowMenu();
    }
}
