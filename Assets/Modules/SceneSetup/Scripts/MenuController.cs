using UnityEngine;
using Zenject;

public abstract class MenuController : MonoBehaviour
{
    [Inject] protected ISceneSetupModel _sceneSetupModel;
    [SerializeField] protected GameObject _visuals;
    [SerializeField] protected float _heightOffsetUserHeadMenu = 0.2f;

    protected virtual void ShowMenu()
    {
        MakeHeightMatchUserHeight();

        // TODO maybe make it appear/disappear by triggering an animation instead
        _visuals.SetActive(true);
    }

    protected void HideMenu()
    {
        _visuals.SetActive(false);
    }

    protected void MakeHeightMatchUserHeight()
    {
        if (_sceneSetupModel.UserHead == null)
        {
            return;
        }

        var userHeight = _sceneSetupModel.UserHead.position.y;
        transform.position += Vector3.up * (userHeight - transform.position.y - _heightOffsetUserHeadMenu);
    }
}
