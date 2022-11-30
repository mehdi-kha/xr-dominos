using System.Collections.Generic;
using UnityEngine;

public class BowlLeavingDominosController : MonoBehaviour
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private string _dominoTag = "Domino";

    public Dictionary<Collider, DominoController> DominosInBowl = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != _dominoTag)
        {
            return;
        }

        DominoController dominoController;
        if (!other.TryGetComponent<DominoController>(out dominoController))
        {
            dominoController = other.GetComponent<DominoControllerRef>().DominoController;
        }
        DominosInBowl[other] = dominoController;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != _dominoTag)
        {
            return;
        }

        DominosInBowl.Remove(other);
    }
}
