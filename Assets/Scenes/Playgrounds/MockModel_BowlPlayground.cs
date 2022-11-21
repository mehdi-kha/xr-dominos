using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MockModel_BowlPlayground : MonoBehaviour
{
    [Inject] private SceneSetupModel _sceneSetupModel;
    private void Start()
    {
        _sceneSetupModel.HasGameStarted = true;
    }
}
