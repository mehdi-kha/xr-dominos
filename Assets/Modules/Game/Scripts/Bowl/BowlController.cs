using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class BowlController : MonoBehaviour, IBowl
{
    public IDesk CorrespondingDesk;
    public List<DominoController> DominosInBowl => _bowlLeavingDominosController.DominosInBowl.Values.ToList();
    public int NumberOfDominosToSpawn => _numberOfDominosToSpawn;

    [Inject] private ISceneSetupModel _sceneSetupModel;
    [SerializeField] private GameObject _visuals;
    [SerializeField] private Transform _dominoSpawningSpot;
    [Tooltip("If true, some horizontal vector with a random direction will be applied to the spawning spot for each domino being spawned.")]
    [SerializeField] private bool _addRandomnessAroundTheSpawningSpot = true;
    [Tooltip("Only relevant is spawning randomness is enabled. Max distance from the defined spawning spot.")]
    [SerializeField] private float _spawningRandomnessRadius = 0.05f;
    [SerializeField] private int _numberOfDominosToSpawn = 10;
    [SerializeField] private BowlLeavingDominosController _bowlLeavingDominosController;

    private void Awake()
    {
        _sceneSetupModel.GameStarted += OnGameStarted;
    }

    private void OnDestroy()
    {
        _sceneSetupModel.GameStarted -= OnGameStarted;
    }

    private void Start()
    {
        if (!_sceneSetupModel.HasGameStarted.ContainsKey(CorrespondingDesk) || !_sceneSetupModel.HasGameStarted[CorrespondingDesk])
        {
            Hide();
        }
    }

    private Vector3 RandomizeSpawningPosition(Vector3 spawningPosition)
    {
        var randomRotation = UnityEngine.Random.Range(0, 360);
        var randomVector = Quaternion.AngleAxis(randomRotation, Vector3.up) * Vector3.right;
        spawningPosition += UnityEngine.Random.Range(0, 0.05f) * randomVector;
        return spawningPosition;
    }

    private void OnGameStarted(IDesk desk)
    {
        if (desk != CorrespondingDesk)
        {
            return;
        }

        Show();
    }

    private void Hide()
    {
        //TODO improve this with animations
        _visuals.SetActive(false);
    }

    private void Show()
    {
        _visuals.SetActive(true);
    }

    public Vector3 GetDominoSpawningPosition()
    {
        if (_addRandomnessAroundTheSpawningSpot)
        {
            return RandomizeSpawningPosition(_dominoSpawningSpot.position);
        }

        return _dominoSpawningSpot.position;
    }
}
