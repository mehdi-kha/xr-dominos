using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class FootstepsController : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [SerializeField] private string _playerColliderTag = "MainCamera";
    [SerializeField] private Material _footPrintsMaterial;
    [SerializeField] private ColorReference _colorWhenUserOver;
    private Color _colorWhenUserNotOver;

    private void Awake()
    {
        _colorWhenUserNotOver = _footPrintsMaterial.color;
    }

    private void OnDestroy()
    {
        _footPrintsMaterial.color = _colorWhenUserNotOver;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Contains(_playerColliderTag))
        {
            _sceneSetupModel.IsUserOnFootsteps = true;
            _footPrintsMaterial.color = _colorWhenUserOver.Value;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag.Contains(_playerColliderTag))
        {
            _sceneSetupModel.IsUserOnFootsteps = false;
            _footPrintsMaterial.color = _colorWhenUserNotOver;
        }
    }
}
