using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SceneSetupModel : ISceneSetupModel
{
    public event Action SkipRoomConfiguration;
    public event Action NoSceneModelToLoad;

    public void RaiseSkipRoomConfiguration()
    {
        SkipRoomConfiguration?.Invoke();
    }

    public void RaiseNoSceneModelToLoad()
    {
        NoSceneModelToLoad?.Invoke();
    }

    public void CloseApp()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}
