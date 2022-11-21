using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SceneSetupModel : ISceneSetupModel
{
    private bool _isUserOnFootsteps;
    public bool IsUserOnFootsteps
    { 
        get => _isUserOnFootsteps; 
        set
        {
            _isUserOnFootsteps = value;
            UserFootstepsStatusChanged?.Invoke(value);
        }
    }

    public event Action SkipRoomConfiguration;
    public event Action DeskSpawned;
    public event Action GameStarted;
    public event Action<bool> UserFootstepsStatusChanged;

    public void RaiseSkipRoomConfiguration()
    {
        SkipRoomConfiguration?.Invoke();
    }

    public void CloseApp()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    public void RaiseGameStarted()
    {
        GameStarted?.Invoke();
    }

    public SceneSetupModel()
    {
        DeskSpawnedRaiser.DeskSpawned += () => DeskSpawned.Invoke();
    }
}
