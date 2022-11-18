using UnityEngine;
using Zenject;

public class SceneSetupAPIController : MonoBehaviour
{
    [SerializeField] private OVRSceneManager _ovrSceneManager;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [Header("The default table must have it's pivot on the floor")]
    [SerializeField] private GameObject _defaultTable;
    [SerializeField] private Vector3 defaultTableSpawningPosition = new Vector3(0, 0, 1);

    void Awake()
    {
        _ovrSceneManager.NoSceneModelToLoad += () => _sceneSetupModel.RaiseNoSceneModelToLoad();
        _sceneSetupModel.SkipRoomConfiguration += OnSkipRoomConfiguration;
    }

    private void OnSkipRoomConfiguration()
    {
        InstantiateDefaultTable();
    }

    private void InstantiateDefaultTable()
    {
        var defaultTable = Instantiate(_defaultTable);
        defaultTable.transform.position = defaultTableSpawningPosition;
    }
}
