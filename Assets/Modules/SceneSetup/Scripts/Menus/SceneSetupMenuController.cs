using Oculus.Interaction;
using System;
using UnityEngine;

public class SceneSetupMenuController : MenuController
{
    [Header("No room configured")]
    [SerializeField] private GameObject _noRoomConfiguredPanel;
    /// <summary>
    ///     Triggering the room setup by calling Oculus' method OVRSceneManager.RequestSceneCapture() was not working well,
    ///     hence having a message indicating to the user to quit the app and do the setup manually instead for now.
    /// </summary>
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _quitButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _continueButton;

    void Awake()
    {
        _sceneSetupModel.DeskDetected += OnDeskSpawned;
        _sceneSetupModel.SkipRoomConfiguration += OnSkipRoomConfiguration;

        // Setup buttons
        _quitButton.WhenPointerEventRaised += OnQuitButtonEventRaised;
        _continueButton.WhenPointerEventRaised += OnContinueButtonEventRaised;
    }

    private void Update()
    {
        if (!isVisible)
        {
            return;
        }

        MakeHeightMatchUserHeight();
    }

    void OnDestroy()
    {
        _sceneSetupModel.DeskDetected -= OnDeskSpawned;
        _sceneSetupModel.SkipRoomConfiguration -= OnSkipRoomConfiguration;

        _quitButton.WhenPointerEventRaised -= OnQuitButtonEventRaised;
        _continueButton.WhenPointerEventRaised -= OnContinueButtonEventRaised;
    }

    private void OnQuitButtonEventRaised(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }

        _sceneSetupModel.CloseApp();
    }

    private void OnContinueButtonEventRaised(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }

        _sceneSetupModel.RaiseSkipRoomConfiguration();
    }

    private void OnSkipRoomConfiguration()
    {
        HideMenu();
    }

    private void OnDeskSpawned(DeskController deskController)
    {
        HideMenu();
    }
}
