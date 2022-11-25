using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class NonPlayableDominoFallingDector : MonoBehaviour
{
    [SerializeField] private string _nonPlayableDominoTag = "NonPlayableDomino";
    [Inject] private IGameModel _gameModel;

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

        _gameModel.HasAtLeastOneNonPlayableDominoFallen = true;

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
