using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SceneSetupModel : ISceneSetupModel
{
    private bool _isUserOnFootsteps;
    private Transform _userHead;
    [SerializeField] private string _defaultPlayerHeadTag = "MainCamera";
    private List<DeskController> _deskControllers = new();
    public bool IsUserOnFootsteps
    { 
        get => _isUserOnFootsteps; 
        set
        {
            _isUserOnFootsteps = value;
            UserFootprintsStatusChanged?.Invoke(value);
        }
    }

    public bool HaveDesksBeenDetected { get; private set; }
    public Transform UserHead
    { 
        get 
        {
            if (_userHead == null)
            {
                var foundUserHead = GameObject.FindGameObjectWithTag(_defaultPlayerHeadTag);
                if (foundUserHead == null)
                {
                    throw new NullReferenceException($"{nameof(SceneSetupModel)}: No gameobject with the tag {_defaultPlayerHeadTag} could be found when assigning UserHead");
                }
                _userHead = foundUserHead.transform;
            }
            return _userHead;
        } 
        set 
        {
            _userHead = value;
        } 
    }

    public event Action SkipRoomConfiguration;
    public event Action DeskDetected;
    public event Action GameStarted;
    public event Action<bool> UserFootprintsStatusChanged;

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

        // It's complicated to inject this model into the DeskController, since the prefabs are spawned by the Oculus API.
        // Hence exceptionally calling the controller's methods here
        foreach(var deskController in _deskControllers)
        {
            deskController.Show();
        }
    }

    public SceneSetupModel()
    {
        DeskController.DeskSpawned += (deskController) =>
        {
            DeskDetected.Invoke();
            HaveDesksBeenDetected = true;
            _deskControllers.Add(deskController);
        };
    }
}
