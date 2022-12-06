using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MainInstaller", menuName = "Installers/MainInstaller")]
public class MainInstaller : ScriptableObjectInstaller<MainInstaller>
{
    public GameObject BowlPrefab;
    public GameObject NonPlayableDominoFallingDetectorPrefab;
    public GameObject DominoPrefab;
    public SceneSetupModel _sceneSetupModel;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneSetupModel>().FromInstance(_sceneSetupModel);
        Container.BindInterfacesAndSelfTo<GameModel>().AsSingle();
        Container.BindFactory<BowlController, BowlFactory>().FromComponentInNewPrefab(BowlPrefab);
        Container.BindFactory<DominoController, DominoFactory>().FromComponentInNewPrefab(DominoPrefab);
        Container.BindFactory<NonPlayableDominoFallingDetector, NonPlayableDominoFallingDetectorFactory>().FromComponentInNewPrefab(NonPlayableDominoFallingDetectorPrefab);
        Container.BindInterfacesAndSelfTo<RendererVisibilityUtil>().AsSingle();
    }
}