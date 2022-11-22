using Oculus.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class BowlController : MonoBehaviour
{
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [SerializeField] private GameObject _visuals;
    [Tooltip("Very probably a box collider that is a trigger. Whenever a domino leaves this collider, a new one is spawned")]
    [SerializeField] private Collider _triggerCollider;
    [SerializeField] private GameObject _dominoPrefab;
    [SerializeField] private string _dominoTag = "Domino";
    [SerializeField] private Transform _dominoSpawningSpot;
    [Tooltip("If true, some horizontal vector with a random direction will be applied to the spawning spot for each domino being spawned.")]
    [SerializeField] private bool _addRandomnessAroundTheSpawningSpot = true;
    [Tooltip("Only relevant is spawning randomness is enabled. Max distance from the defined spawning spot.")]
    [SerializeField] private float _spawningRandomnessRadius = 0.05f;
    [SerializeField] private int _minimumNumberOfDominosInBowl = 10;

    private ObjectPool<GameObject> _dominoPool;
    // Using a dictionary to avoid registering a domino instance twice by mistake.
    private Dictionary<int, Collider> _dominosInBowl = new();

    private void Awake()
    {
        _dominoPool = new ObjectPool<GameObject>(CreateDomino, OnTakeDominoFromPool, null, null, true, 20);
        if (!_sceneSetupModel.HasGameStarted)
        {
            Hide();
        }

        _sceneSetupModel.GameStarted += OnGameStarted;
        if (_triggerCollider == null || !_triggerCollider.isTrigger)
        {
            throw new Exception($"{nameof(BowlController)}: Either the trigger collider is not assigned, or it's not of type trigger");
        }
    }

    private void PopulateBowl()
    {
        while(_dominosInBowl.Count < _minimumNumberOfDominosInBowl)
        {
            _dominoPool.Get();
        }
    }

    private GameObject CreateDomino()
    {
        return Instantiate(_dominoPrefab);
    }

    private void OnTakeDominoFromPool(GameObject domino)
    {
        var dominoPosition = _dominoSpawningSpot.position;
        if (_addRandomnessAroundTheSpawningSpot)
        {
            dominoPosition = RandomizeSpawningPosition(dominoPosition);
        }
        domino.transform.position = dominoPosition;

        // OnTriggerEnter won't be called if spawning happens during Awake() or Start() for example
        // hence updating the dictionary of dominos in the bowl here as well
        var dominosCollider = domino.GetComponentInChildren<Collider>();
        _dominosInBowl[dominosCollider.GetInstanceID()] = dominosCollider;
        domino.GetComponent<Grabbable>().WhenPointerEventRaised += (a) =>
        {
            if (a.Type == PointerEventType.Select)
            {
                domino.GetComponent<Rigidbody>().velocity = Vector3.zero;
                domino.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                
            }
        };
    }

    private Vector3 RandomizeSpawningPosition(Vector3 spawningPosition)
    {
        var randomRotation = UnityEngine.Random.Range(0, 360);
        var randomVector = Quaternion.AngleAxis(randomRotation, Vector3.up) * Vector3.right;
        spawningPosition += UnityEngine.Random.Range(0, 0.05f) * randomVector;
        return spawningPosition;
    }

    private void OnGameStarted()
    {
        ShowAndPopulateWithDominos();
    }

    private void Hide()
    {
        //TODO improve this with animations
        _visuals.SetActive(false);
    }

    private void ShowAndPopulateWithDominos()
    {
        _visuals.SetActive(true);
        PopulateBowl();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != _dominoTag)
        {
            return;
        }

        _dominosInBowl.Remove(other.GetInstanceID());
        PopulateBowl();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != _dominoTag)
        {
            return;
        }

        _dominosInBowl[other.GetInstanceID()] = other;
    }
}
