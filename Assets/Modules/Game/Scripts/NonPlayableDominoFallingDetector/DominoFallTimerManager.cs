using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DominoFallTimerManager : MonoBehaviour
{
    public IDesk CorrespondingDesk;
    [Inject] private IGameModel _gameModel;
    [Tooltip("The maximum time that can pass between a non playable domino falling and the game considered being finished.")]
    [SerializeField] private float _countdownInitialValueSec = 2;

    private float _countdown;
    private bool _isCountdownRunning;

    private void Awake()
    {
        _gameModel.DominoFellDown += OnDominoFellDown;
    }

    private void OnDestroy()
    {
        _gameModel.DominoFellDown -= OnDominoFellDown;
    }

    private void OnDominoFellDown(IDomino domino)
    {
        if (domino.IsPlayableDomino)
        {
            return;
        }

        _gameModel.SetHasAtLeastOneNonPlayableDominoFallen(CorrespondingDesk, true);
        ResetAndStartCountdown();
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
            _gameModel.SetIsFallingCountdownFinished(CorrespondingDesk, true);
            _isCountdownRunning = false;
        }
    }
}
