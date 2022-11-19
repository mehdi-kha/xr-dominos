using System;

public interface ISceneSetupModel
{
    public event Action SkipRoomConfiguration;
    public event Action DeskSpawned;
    public event Action GameStarted;
    public void RaiseSkipRoomConfiguration();
    public void CloseApp();
    public void RaiseGameStarted();
}
