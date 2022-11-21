using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MainInstaller", menuName = "Installers/MainInstaller")]
public class MainInstaller : ScriptableObjectInstaller<MainInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneSetupModel>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameModel>().AsSingle();
    }
}