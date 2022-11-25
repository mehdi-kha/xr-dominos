using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallingDominoDetector : MonoBehaviour
{
    [SerializeField] private string _nonPlayableDominoTag = "NonPlayableDomino";
    /// <summary>
    ///     Contains the non playable dominos. The value is true if the domino has fallen.
    /// </summary>
    private Dictionary<int, bool> _fallenDominos = new();
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != _nonPlayableDominoTag)
        {
            return;
        }

        _fallenDominos[other.GetHashCode()] = true;

        if (_fallenDominos.All(a => a.Value))
        {
            Debug.Log("All non playable dominos have fallen down!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != _nonPlayableDominoTag)
        {
            return;
        }

        _fallenDominos[other.GetHashCode()] = false;
    }
}
