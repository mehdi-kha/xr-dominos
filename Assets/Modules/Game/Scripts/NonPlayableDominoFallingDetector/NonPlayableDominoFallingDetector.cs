using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class NonPlayableDominoFallingDetector : MonoBehaviour
{
    public IDesk CorrespondingDesk;
    [SerializeField] private string _nonPlayableDominoTag = "NonPlayableDomino";
    [Inject] private IGameModel _gameModel;
    [Tooltip("The maximum time that can pass between a non playable domino falling and the game considered being finished.")]
    [SerializeField] private float _countdownInitialValueSec = 2;

    private float _countdown;
    private bool _isCountdownRunning;

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

        _gameModel.SetHasAtLeastOneNonPlayableDominoFallen(CorrespondingDesk, true);
        ResetAndStartCountdown();

        if (_fallenDominos.All(a => a.Value))
        {
            _gameModel.SetHaveAllNonPlayableDominosFallen(CorrespondingDesk, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != _nonPlayableDominoTag)
        {
            return;
        }

        _fallenDominos[other.GetHashCode()] = false;
        _gameModel.SetHaveAllNonPlayableDominosFallen(CorrespondingDesk, false);
    }

    private void ResetAndStartCountdown()
    {
        _countdown = _countdownInitialValueSec;
        _isCountdownRunning = true;
    }

    private void Update()
    {
        if (!_isCountdownRunning)
        {
            return;
        }

        _countdown -= Time.deltaTime;
        if (_countdown <= 0)
        {
            _gameModel.SetIsFallingCOuntdownFinished(CorrespondingDesk, true);
            _isCountdownRunning = false;
        }
    }
}
