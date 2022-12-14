using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class FootstepsController : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Inject] private VisibilityUtil _visibilityUtil;
    [SerializeField] private string _playerColliderTag = "MainCamera";
    [SerializeField] private Material _footPrintsMaterial;
    [SerializeField] private ColorReference _colorWhenUserOver;
    [SerializeField] private TextMeshProUGUI[] _panels;
    private Color _colorWhenUserNotOver;

    private void Awake()
    {
        _colorWhenUserNotOver = _footPrintsMaterial.color;
        _sceneSetupModel.GameStarted += OnGameStarted;
    }

    private void ShowPanels()
    {
        foreach (var panel in _panels)
        {
            _visibilityUtil.ShowTmpText(panel);
        }
    }

    private void HidePanels()
    {
        foreach (var panel in _panels)
        {
            _visibilityUtil.HideTmpText(panel);
        }
    }

    private void OnGameStarted(IDesk desk)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _footPrintsMaterial.color = _colorWhenUserNotOver;
        _sceneSetupModel.GameStarted -= OnGameStarted;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Contains(_playerColliderTag))
        {
            _sceneSetupModel.IsUserOnFootsteps = true;
            _footPrintsMaterial.color = _colorWhenUserOver.Value;
            HidePanels();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag.Contains(_playerColliderTag))
        {
            _sceneSetupModel.IsUserOnFootsteps = false;
            _footPrintsMaterial.color = _colorWhenUserNotOver;
            ShowPanels();
        }
    }
}
