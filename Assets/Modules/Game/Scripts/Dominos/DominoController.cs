using Oculus.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPointable))]
public class DominoController : MonoBehaviour, IDomino
{
    public event Action<DominoController> OnGrabbed;
    public event Action<DominoController> OnReleased;
    public IBowl CorrespondingBowl;
    public IDesk CorrespondingDesk { get; set; }
    public IGameModel GameModel;

    [SerializeField] private List<GameObject> _grabInteractables;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _visuals;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _dissolveAnimationTrigger = "DissolveDomino";
    [SerializeField] private string _showupAnimationTrigger = "ShowDomino";
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _horizontalThreshold = 45;
    [SerializeField] private bool _isPlayableDomino = true;
    private IPointable _pointable;
    private bool _hasFallenDown;

    public bool HasFallenDown => _hasFallenDown;

    public Transform Transform => transform;

    public bool IsPlayableDomino => _isPlayableDomino;

    private void Awake()
    {
        _pointable = GetComponent<IPointable>();
        _pointable.WhenPointerEventRaised += OnPointerEventRaised;
    }

    private void OnDestroy()
    {
        _pointable.WhenPointerEventRaised -= OnPointerEventRaised;
    }

    private void OnPointerEventRaised(PointerEvent pointerEvent)
    {
        if (pointerEvent.Type == PointerEventType.Select)
        {
            OnGrabbed?.Invoke(this);
        }

        if (pointerEvent.Type == PointerEventType.Unselect)
        {
            OnReleased?.Invoke(this);
        }
    }

    public void MakeNonInteractable()
    {
        foreach (var grabInteractable in _grabInteractables)
        {
            grabInteractable.gameObject.SetActive(false);
        }
    }

    public void MakeInteractable()
    {
        foreach (var grabInteractable in _grabInteractables)
        {
            grabInteractable.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        _rigidBody.isKinematic = true;
        _animator.SetTrigger(_dissolveAnimationTrigger);
        _particleSystem.Play();
    }

    public void Show()
    {
        _animator.SetTrigger(_showupAnimationTrigger);
        _rigidBody.isKinematic = false;
    }

    public void SetActive(bool shouldBeActive)
    {
        gameObject.SetActive(shouldBeActive);
    }

    public void SetPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }

    public void SetBowl(IBowl bowl)
    {
        CorrespondingBowl = bowl;
    }

    private void Update()
    {
        if (IsPlayableDomino)
        {
            return;
        }

        var hasFallenDown = EvaluateIsHorizontal();
        if (hasFallenDown && !_hasFallenDown)
        {
            HandleDominoFallenDown();
        }

        _hasFallenDown = hasFallenDown;
    }

    private void HandleDominoFallenDown()
    {
        GameModel.SetDominoFellDown(this);
    }

    private bool EvaluateIsHorizontal()
    {
        float xAngle = transform.eulerAngles.x;
        if (xAngle > 180.0f) xAngle -= 360.0f;

        float zAngle = transform.eulerAngles.z;
        if (zAngle > 180.0f) zAngle -= 360.0f;

        return Mathf.Abs(xAngle) > _horizontalThreshold || Mathf.Abs(zAngle) > _horizontalThreshold;
    }
}
