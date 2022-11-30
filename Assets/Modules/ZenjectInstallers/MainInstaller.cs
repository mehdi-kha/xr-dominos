using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MainInstaller", menuName = "Installers/MainInstaller")]
public class MainInstaller : ScriptableObjectInstaller<MainInstaller>
{
    public GameObject BowlPrefab;
    public GameObject NonPlayableDominoFallingDetectorPrefab;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneSetupModel>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameModel>().AsSingle();
        Container.BindFactory<DominosController, DominosFactory>().FromComponentInNewPrefab(BowlPrefab);
        Container.BindFactory<NonPlayableDominoFallingDetector, NonPlayableDominoFallingDetectorFactory>().FromComponentInNewPrefab(NonPlayableDominoFallingDetectorPrefab);
    }
}