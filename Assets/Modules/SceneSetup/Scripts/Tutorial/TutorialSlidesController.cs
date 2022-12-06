using Oculus.Interaction;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class TutorialSlidesController : MonoBehaviour
{
    [SerializeField] private SlideController _slide;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _previousSlideButton;
    [SerializeField] private PointerInteractable<PokeInteractor, PokeInteractable> _nextSlideButton;
    [SerializeField] private TextMeshPro _nextSlideButtonText;
    [Tooltip("When we have reached the last slide, this text will be displayed on the next button.")]
    [SerializeField] private string _startGameButtonText = "Let's go!";

    private string _nextSlideButtonOriginalText;
    private bool _wasTutorialStarted;
    void Awake()
    {
        _slide.Hide();
        HideButton(_previousSlideButton);
        HideButton(_nextSlideButton);
        _sceneSetupModel.TutorialStarted += OnTutorialStarted;
        _sceneSetupModel.OnCurrentTutorialSlideChanged += OnCurrentTutorialSlideChanged;
        _sceneSetupModel.GameStarted += OnGameStarted;
        _previousSlideButton.WhenPointerEventRaised += OnPreviousButtonClicked;
        _nextSlideButton.WhenPointerEventRaised += OnNextButtonClicked;
        _nextSlideButtonOriginalText = _nextSlideButtonText.text;
    }

    private void OnDestroy()
    {
        _sceneSetupModel.TutorialStarted -= OnTutorialStarted;
        _sceneSetupModel.OnCurrentTutorialSlideChanged -= OnCurrentTutorialSlideChanged;
        _sceneSetupModel.GameStarted -= OnGameStarted;
        _previousSlideButton.WhenPointerEventRaised -= OnPreviousButtonClicked;
        _nextSlideButton.WhenPointerEventRaised -= OnNextButtonClicked;
    }

    private void HideButton(PointerInteractable<PokeInteractor, PokeInteractable> button)
    {
        button.gameObject.SetActive(false);
    }

    private void ShowButton(PointerInteractable<PokeInteractor, PokeInteractable> button)
    {
        if (!_wasTutorialStarted)
        {
            return;
        }

        button.gameObject.SetActive(true);
    }

    private void OnTutorialStarted()
    {
        _wasTutorialStarted = true;
        var currentSlide = _sceneSetupModel.CurrentTutorialSlide;
        ShowSlide(currentSlide);
        UpdateButtonsVisibility(currentSlide);
    }

    private void OnGameStarted(IDesk obj)
    {
        _slide.Hide();
        HideButton(_previousSlideButton);
        HideButton(_nextSlideButton);
    }

    private void OnCurrentTutorialSlideChanged(LinkedListNode<SlideData> currentSlide)
    {
        ShowSlide(currentSlide);
        UpdateButtonsVisibility(currentSlide);
    }

    private void OnNextButtonClicked(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }

        if (_sceneSetupModel.CurrentTutorialSlide.Next == null)
        {
            _sceneSetupModel.StartGameForAllDesks();
            return;
        }

        _sceneSetupModel.CurrentTutorialSlide = _sceneSetupModel.CurrentTutorialSlide.Next;
    }

    private void OnPreviousButtonClicked(PointerEvent obj)
    {
        if (obj.Type != PointerEventType.Select)
        {
            return;
        }

        _sceneSetupModel.CurrentTutorialSlide = _sceneSetupModel.CurrentTutorialSlide.Previous;
    }

    private void UpdateButtonsVisibility(LinkedListNode<SlideData> currentSlide)
    {
        if (currentSlide.Previous == null)
        {
            HideButton(_previousSlideButton);
        }

        else
        {
            ShowButton(_previousSlideButton);
        }

        if (currentSlide.Next == null)
        {
            _nextSlideButtonText.text = _startGameButtonText;
            ShowButton(_nextSlideButton);
        }

        else
        {
            _nextSlideButtonText.text = _nextSlideButtonOriginalText;
            ShowButton(_nextSlideButton);
        }
    }

    private void ShowSlide(LinkedListNode<SlideData> currentTutorialSlide)
    {
        if (!_wasTutorialStarted)
        {
            return;
        }

        _slide.Show(currentTutorialSlide.Value);
    }
}
