using UnityEngine;

/// <summary>
///     Disables the rig used on the device
/// </summary>
public class RigDisabler : MonoBehaviour
{
    [SerializeField] private GameObject _deviceRig;
    [SerializeField] private GameObject _editorRig;

    // Start is called before the first frame update
    void Awake()
    {
        if (Application.isEditor)
        {
            _editorRig.SetActive(true);
            return;
        }
        _deviceRig.SetActive(true);
    }
}
