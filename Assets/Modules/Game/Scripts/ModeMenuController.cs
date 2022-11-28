using Oculus.Interaction;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ModeMenuController : MenuController
{
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _arModeButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _vrModeButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _quitGameButton;
    [SerializeField] private TextMeshProUGUI _currentModeText;
    [SerializeField] private string _arModeText = "Current mode: <br>AR Mode";
    [SerializeField] private string _vrModeText = "Current mode: <br>VR Mode";
    [SerializeField] private ColorReference _vrCameraBackground;
    [SerializeField] private ColorReference _arCameraBackground;
    [Inject] private IGameModel _gameModel;

    [SerializeField] private List<Camera> _camerasToModify;
    void Awake()
    {
        HideMenu();
        _sceneSetupModel.GameStarted += OnGameStarted;
        _arModeButton.WhenPointerEventRaised += OnARModeButtonPointerRaised;
        _vrModeButton.WhenPointerEventRaised += OnVRModeButtonPointerRaised;
        _quitGameButton.WhenPointerEventRaised += OnQuitGameButtonPointerRaised;
        _gameModel.XRModeChanged += OnXRModeChanged;
    }

    private void OnQuitGameButtonPointerRaised(PointerEvent obj)
    {
        _sceneSetupModel.CloseApp();
    }

    private void OnXRModeChanged(XRMode currentGameMode)
    {
        UpdateMenuText(currentGameMode);
        var color = currentGameMode == XRMode.ARMode ? _arCameraBackground : _vrCameraBackground;
        foreach (var camera in _camerasToModify)
        {
            camera.backgroundColor = color.Value;
        }
    }

    private void UpdateMenuText(XRMode currentGameMode)
    {
        _currentModeText.text = currentGameMode == XRMode.ARMode ? _arModeText : _vrModeText;
    }

    private void OnVRModeButtonPointerRaised(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }
        _gameModel.CurrentXRMode = XRMode.VRMode;
    }

    private void OnARModeButtonPointerRaised(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }
        _gameModel.CurrentXRMode = XRMode.ARMode;
    }

    private void OnGameStarted(IDesk desk)
    {
        ShowMenu();
    }

    protected override void ShowMenu()
    {
        base.ShowMenu();
        UpdateMenuText(_gameModel.CurrentXRMode);
    }

    private void OnDestroy()
    {
        _sceneSetupModel.GameStarted -= OnGameStarted;
        _arModeButton.WhenPointerEventRaised -= OnARModeButtonPointerRaised;
        _vrModeButton.WhenPointerEventRaised -= OnVRModeButtonPointerRaised;
        _quitGameButton.WhenPointerEventRaised -= OnQuitGameButtonPointerRaised;
        _gameModel.XRModeChanged -= OnXRModeChanged;
    }
}
