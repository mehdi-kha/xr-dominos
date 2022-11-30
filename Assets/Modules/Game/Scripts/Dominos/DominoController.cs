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

    [SerializeField] private List<GameObject> _grabInteractables;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _visuals;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _dissolveAnimationTrigger = "DissolveDomino";
    [SerializeField] private string _showupAnimationTrigger = "ShowDomino";
    [SerializeField] private Rigidbody _rigidBody;
    private IPointable _pointable;

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
}
