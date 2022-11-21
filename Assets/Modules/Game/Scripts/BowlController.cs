using UnityEngine;
using Zenject;

public class BowlController : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [SerializeField] private GameObject _visuals;

    private void Awake()
    {
        if (!_sceneSetupModel.HasGameStarted)
        {
            Hide();
        }

        _sceneSetupModel.GameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
        Show();
    }

    private void Hide()
    {
        //TODO improve this with animations
        _visuals.SetActive(false);
    }

    private void Show()
    {
        _visuals.SetActive(true);
    }
}
