using System;
using UnityEngine;
using Zenject;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip _genericAudioClip;
    [SerializeField] private AudioClip _gameAudioClip;
    [SerializeField] private AudioClip _levelSucceededAudioClip;
    [SerializeField] private AudioClip _levelFailedAudioClip;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _voiceOverAudioSource;

    [Inject] private ISceneSetupModel _sceneModelSetup;
    [Inject] private IGameModel _gameModel;

    void Start()
    {
        StartMusicAudioClip(_genericAudioClip);
        _sceneModelSetup.GameStarted += OnGameStarted;
        _gameModel.LevelSucceeded += OnLevelSucceeded;
        _gameModel.LevelFailed += OnLevelFailed;
    }

    private void OnDestroy()
    {
        _sceneModelSetup.GameStarted -= OnGameStarted;
        _gameModel.LevelSucceeded -= OnLevelSucceeded;
        _gameModel.LevelFailed -= OnLevelFailed;
    }

    private void OnLevelFailed(IDesk obj)
    {
        StartVoiceOverAudioClip(_levelFailedAudioClip);
    }

    private void OnLevelSucceeded(IDesk obj)
    {
        StartVoiceOverAudioClip(_levelSucceededAudioClip);
    }

    private void OnGameStarted(IDesk obj)
    {
        StartMusicAudioClip(_gameAudioClip);
    }

    private void StartMusicAudioClip(AudioClip audioClip)
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = audioClip;
        _musicAudioSource.Play();
    }

    private void StartVoiceOverAudioClip(AudioClip audioClip)
    {
        _voiceOverAudioSource.Stop();
        _voiceOverAudioSource.clip = audioClip;
        _voiceOverAudioSource.Play();
    }
}
