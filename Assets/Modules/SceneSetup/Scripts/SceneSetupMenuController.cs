using Oculus.Interaction;
using System;
using UnityEngine;
using Zenject;

public class SceneSetupMenuController : MonoBehaviour
{
    [Header("No room configured")]
    [SerializeField] private GameObject _noRoomConfiguredPanel;
    /// <summary>
    ///     Triggering the room setup by calling Oculus' method OVRSceneManager.RequestSceneCapture() was not working well,
    ///     hence having a message indicating to the user to quit the app and do the setup manually instead for now.
    /// </summary>
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _quitButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _continueButton;
    [Inject] private ISceneSetupModel _roomConfigurationModel;

    void Awake()
    {
        _roomConfigurationModel.NoSceneModelToLoad += OnNoSceneModelToLoad;
        _roomConfigurationModel.SkipRoomConfiguration += OnSkipRoomConfiguration;

        // Setup buttons
        _quitButton.WhenPointerEventRaised += OnQuitButtonEventRaised;
        _continueButton.WhenPointerEventRaised += OnContinueButtonEventRaised;
    }

    void OnDestroy()
    {
        _roomConfigurationModel.NoSceneModelToLoad -= OnNoSceneModelToLoad;
        _roomConfigurationModel.SkipRoomConfiguration -= OnSkipRoomConfiguration;

        _quitButton.WhenPointerEventRaised -= OnQuitButtonEventRaised;
        _continueButton.WhenPointerEventRaised -= OnContinueButtonEventRaised;
    }

    private void OnQuitButtonEventRaised(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }

        _roomConfigurationModel.CloseApp();
    }

    private void OnContinueButtonEventRaised(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }

        _roomConfigurationModel.RaiseSkipRoomConfiguration();
    }

    private void OnSkipRoomConfiguration()
    {
        _noRoomConfiguredPanel.SetActive(false);
    }

    private void OnNoSceneModelToLoad()
    {
        _noRoomConfiguredPanel.SetActive(true);
    }
}
