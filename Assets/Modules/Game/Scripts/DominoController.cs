using System.Collections.Generic;
using UnityEngine;

public class DominoController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _grabInteractables;
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
}
