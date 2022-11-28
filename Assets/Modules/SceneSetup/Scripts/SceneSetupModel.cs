using System;
using System.Collections;
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
    private Dictionary<IDesk, bool> _hasGameStarted = new();
    private List<IDesk> _desks = new();
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

    public IReadOnlyDictionary<IDesk, bool> HasGameStarted => _hasGameStarted;

    public void StartGameForAllDesks()
    {
        foreach (var desk in _desks)
        {
            _hasGameStarted[desk] = true;
            GameStarted?.Invoke(desk);
            desk.Show();
        }
    }

    public IEnumerable<IDesk> Desks => _desks;

    public event Action SkipRoomConfiguration;
    public event Action<DeskController> DeskDetected;
    public event Action<IDesk> GameStarted;
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

    public SceneSetupModel()
    {
        DeskController.DeskSpawned += (deskController) =>
        {
            _desks.Add(deskController);
            DeskDetected?.Invoke(deskController);
            HaveDesksBeenDetected = true;
        };
    }
}
