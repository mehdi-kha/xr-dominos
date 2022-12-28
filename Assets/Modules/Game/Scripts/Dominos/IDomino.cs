using System;
using UnityEngine;

public interface IDomino
{
    public event Action<DominoController> OnGrabbed;
    public event Action<DominoController> OnReleased;
    public void MakeNonInteractable();
    public void MakeInteractable();
    public void Hide();
    public void Show();
    public void SetActive(bool shouldBeActive);
    public void SetPosition(Vector3 worldPosition);
    public void SetBowl(IBowl bowl);
    public bool HasFallenDown { get; }
    public Transform Transform { get; }
    public bool IsPlayableDomino { get; }
    public IDesk CorrespondingDesk { get; set; }
}
