using System;
using UnityEngine;
using Zenject;

public class SceneSetupAPIController : MonoBehaviour
{
    [SerializeField] private OVRSceneManager _ovrSceneManager;
    [Inject] private ISceneSetupModel _sceneSetupModel;

    void Awake()
    {
        _ovrSceneManager.NoSceneModelToLoad += () => _sceneSetupModel.RaiseNoSceneModelToLoad();
    }
}
