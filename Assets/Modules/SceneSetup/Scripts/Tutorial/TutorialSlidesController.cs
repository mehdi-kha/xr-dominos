using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TutorialSlidesController : MonoBehaviour
{
    [SerializeField] private SlideController _slide;
    [Inject] private ISceneSetupModel _sceneSetupModel;
    void Awake()
    {
        _slide.Hide();
        _sceneSetupModel.TutorialStarted += OnTutorialStarted;
    }

    private void OnDestroy()
    {
        _sceneSetupModel.TutorialStarted -= OnTutorialStarted;
    }

    private void OnTutorialStarted()
    {
        ShowSlide(_sceneSetupModel.CurrentTutorialSlide);
    }

    private void ShowSlide(LinkedListNode<SlideData> currentTutorialSlide)
    {
        _slide.Show(currentTutorialSlide.Value);
    }
}
