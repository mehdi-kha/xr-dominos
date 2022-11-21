using System;
using UnityEngine;

public interface ISceneSetupModel
{
    public event Action SkipRoomConfiguration;
    public event Action DeskDetected;
    public event Action GameStarted;
    public event Action<bool> UserFootprintsStatusChanged;
    public void RaiseSkipRoomConfiguration();
    public void CloseApp();
    public void RaiseGameStarted();
    public bool IsUserOnFootsteps { get; set; }
    public bool HaveDesksBeenDetected { get; }
    public Transform UserHead { get; set; }
}
