using UnityEngine;
using Zenject;

public abstract class MenuController : MonoBehaviour
{
    [Inject] protected ISceneSetupModel _sceneSetupModel;
    [SerializeField] protected Animator _visuals;
    [SerializeField] protected float _heightOffsetUserHeadMenu = 0.2f;
    [SerializeField] protected string _showAnimationTrigger = "ShowMenu";
    [SerializeField] protected string _hideAnimationTrigger = "HideMenu";

    protected bool isVisible = true;

    protected virtual void ShowMenu()
    {
        MakeHeightMatchUserHeight();
        _visuals.SetTrigger(_showAnimationTrigger);
        isVisible = true;
    }

    protected void HideMenu()
    {
        _visuals.SetTrigger(_hideAnimationTrigger);
        isVisible = false;
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
