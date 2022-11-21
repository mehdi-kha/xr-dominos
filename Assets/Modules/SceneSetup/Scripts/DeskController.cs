using System;
using UnityEngine;

/// <summary>
///     It's a bit complicated to get a clear list of the desks in the player's zone using the Oculus API,
///     hence this little class to handle our own logic.
/// </summary>
public class DeskController : MonoBehaviour
{
    public static event Action<DeskController> DeskSpawned;
    [SerializeField] private Renderer _renderer;

    void Awake()
    {
        DeskSpawned?.Invoke(this);
        Hide();
    }

    public void Show()
    {
        // TODO maybe an animation would be better
        _renderer.enabled = true;
    }

    public void Hide()
    {
        _renderer.enabled = false;
    }
}
