using System;

public interface ISceneSetupModel
{
    public event Action SkipRoomConfiguration;
    public event Action DeskSpawned;
    public void RaiseSkipRoomConfiguration();
    public void CloseApp();
}
