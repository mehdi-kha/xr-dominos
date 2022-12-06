using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class VisibilityUtil
{
    public enum TransitionMethod
    {
        FadeInOut,
    }

    private Dictionary<object, CancellationTokenSource> _currentTransitions = new();

    /// <summary>
    ///     Simply adjusts the alpha channel of the main material for now. Very simple.
    /// </summary>
    /// <param name="material">The material alpha channel should be adjusted.</param>
    /// <param name="transitionMethod">The transition method to use</param>
    public void ShowMaterial(Material material, TransitionMethod transitionMethod = TransitionMethod.FadeInOut)
    {
        switch(transitionMethod)
        {
            case TransitionMethod.FadeInOut:
                FadeInMaterialAsync(material);
                break;
        }
    }

    public void HideMaterial(Material material, TransitionMethod transitionMethod = TransitionMethod.FadeInOut)
    {
        switch (transitionMethod)
        {
            case TransitionMethod.FadeInOut:
                FadeOutMaterialAsync(material);
                break;
        }
    }

    public void ShowTmpText(TMP_Text tmpText, TransitionMethod transitionMethod = TransitionMethod.FadeInOut)
    {
        switch (transitionMethod)
        {
            case TransitionMethod.FadeInOut:
                FadeInTMPTextAsync(tmpText);
                break;
        }
    }

    public void HideTmpText(TMP_Text tmpText, TransitionMethod transitionMethod = TransitionMethod.FadeInOut)
    {
        switch (transitionMethod)
        {
            case TransitionMethod.FadeInOut:
                FadeOutTMPTextAsync(tmpText);
                break;
        }
    }

    private async void FadeInMaterialAsync(Material material)
    {
        CancelCurrentTransition(material);
        var cts = new CancellationTokenSource();
        _currentTransitions[material] = cts;
        while (!cts.IsCancellationRequested && material.color.a < 1)
        {
            Color currentColor = material.color;
            currentColor.a += 0.025f;
            material.color = currentColor;
            await Task.Delay(50);
        }
    }

    private async void FadeInTMPTextAsync(TMP_Text tmpText)
    {
        CancelCurrentTransition(tmpText);
        var cts = new CancellationTokenSource();
        _currentTransitions[tmpText] = cts;
        while (!cts.IsCancellationRequested && tmpText.alpha < 1)
        {
            tmpText.alpha += 0.05f;
            await Task.Delay(50);
        }
    }

    private async void FadeOutTMPTextAsync(TMP_Text tmpText)
    {
        CancelCurrentTransition(tmpText);
        var cts = new CancellationTokenSource();
        _currentTransitions[tmpText] = cts;
        while (!cts.IsCancellationRequested && tmpText.alpha >= 0)
        {
            tmpText.alpha -= 0.05f;
            await Task.Delay(50);
        }
    }

    private async void FadeOutMaterialAsync(Material material)
    {
        CancelCurrentTransition(material);
        var cts = new CancellationTokenSource();
        _currentTransitions[material] = cts;
        while (!cts.IsCancellationRequested && material.color.a >= 0)
        {
            Color currentColor = material.color;
            currentColor.a -= 0.025f;
            material.color = currentColor;
            await Task.Delay(50);
        }
    }

    private void CancelCurrentTransition(object transition)
    {
        if (_currentTransitions.TryGetValue(transition, out var cts) && cts != null)
        {
            cts.Cancel();
        }
    }
}
