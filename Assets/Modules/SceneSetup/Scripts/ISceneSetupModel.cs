using System;

public interface ISceneSetupModel
{
    public event Action SkipRoomConfiguration;
    public event Action NoSceneModelToLoad;

    public void RaiseNoSceneModelToLoad();
    public void RaiseSkipRoomConfiguration();
    public void CloseApp();
}
