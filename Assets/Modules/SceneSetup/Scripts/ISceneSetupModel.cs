using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneSetupModel
{
    public event Action SkipRoomConfiguration;
    public event Action<DeskController> DeskDetected;
    public event Action<IDesk> GameStarted;
    public event Action<bool> UserFootprintsStatusChanged;
    public void RaiseSkipRoomConfiguration();
    public void CloseApp();
    public IReadOnlyDictionary<IDesk, bool> HasGameStarted { get; }
    public void StartGameForAllDesks();
    public bool IsUserOnFootsteps { get; set; }
    public bool HaveDesksBeenDetected { get; }
    public Transform UserHead { get; set; }
    public IEnumerable<IDesk> Desks { get; }
}
