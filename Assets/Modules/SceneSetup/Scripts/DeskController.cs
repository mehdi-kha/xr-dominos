using System;
using System.Collections;
using UnityEngine;

/// <summary>
///     It's a bit complicated to get a clear list of the desks in the player's zone using the Oculus API,
///     hence this little class to handle our own logic.
/// </summary>
public class DeskController : MonoBehaviour, IDesk
{
    public static event Action<DeskController> DeskSpawned;
    public event Action SetupDone;

    /// <summary>
    ///     The world position of the point at the center on the top of the desk;
    /// </summary>
    public Vector3 CenterTopPosition => transform.position + _boxCollider.bounds.extents.y * transform.up;
    public Bounds Bounds => _meshFilter.mesh.bounds;
    public Transform Transform => transform;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _distanceFromDeskEdge = 0.1f;
    [SerializeField] MeshFilter _meshFilter;
    [SerializeField] BoxCollider _boxCollider;

    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    void Awake()
    {
        DeskSpawned?.Invoke(this);
        StartCoroutine(SetupDeskAfterEndOfFrame());
        HideDeskAndDisableDominoFallingDetector();
    }

    public void Show()
    {
        // TODO maybe an animation would be better
        _renderer.enabled = true;
    }

    private void HideDeskAndDisableDominoFallingDetector()
    {
        _renderer.enabled = false;
    }

    /// <summary>
    ///     Get the spawning spot for the bowl in the local space of the table.
    /// </summary>
    /// <returns>A vector3 representing the position of the bowl in the desk's local space.</returns>
    public Vector3 GetBowlSpawningLocalPosition()
    {
        var meshBounds = _meshFilter.mesh.bounds;
        var minBoundPoint = meshBounds.min;
        var maxBoundPoint = meshBounds.max;
        // Position the bowl at the center of the table mesh
        var localPosition = meshBounds.center;
        // Move it to an edge at the top
        localPosition += new Vector3(
            (minBoundPoint.x + maxBoundPoint.x) / 2,
            maxBoundPoint.y,
            maxBoundPoint.z
            );
        // Move it away from the edge, depending on the _distanceFromDeskEdge that was defined.
        localPosition -= _distanceFromDeskEdge * Vector3.forward;
        return localPosition;
    }

    /// <summary>
    ///     Do this at the end of the frame to make sure the Oculus SDK has finished generating the table's mesh
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetupDeskAfterEndOfFrame()
    {
        yield return _waitForEndOfFrame;
        _boxCollider.center = _meshFilter.mesh.bounds.center;
        _boxCollider.size = _meshFilter.mesh.bounds.size;
        SetupDone?.Invoke();
    }
}
