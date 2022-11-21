using UnityEngine;
using Zenject;

public class CustomCameraRigController : MonoBehaviour
{
    [SerializeField] private Transform _playerHead;
    [Inject] private ISceneSetupModel _sceneModelSetup;

    void Awake()
    {
        _sceneModelSetup.UserHead = _playerHead;
    }
}
