using Oculus.Interaction;
using UnityEngine;
using Zenject;

public class StartGameMenuController : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _startGameButton;
    [SerializeField] private GameObject _visuals;
    private void Awake()
    {
        _sceneSetupModel.DeskSpawned += OnDeskSpawned;
        _startGameButton.WhenPointerEventRaised += OnStartGameButtonPointerEvent;
        _sceneSetupModel.GameStarted += OnGameStarted;
        _visuals.SetActive(false);
    }

    private void OnGameStarted()
    {
        _visuals.SetActive(false);
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
        _visuals.SetActive(true);
    }
}
