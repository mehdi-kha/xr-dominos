using System;
using System.Collections;
using UnityEngine;

/// <summary>
///     It's a bit complicated to get a clear list of the desks in the player's zone using the Oculus API,
///     hence this little class to handle our own logic.
/// </summary>
public class DeskController : MonoBehaviour
{
    public static event Action<DeskController> DeskSpawned;
    public event Action SetupDone;

    /// <summary>
    ///     The world position of the point at the center on the top of the desk;
    /// </summary>
    public Vector3 CenterTopPosition => transform.position + _boxCollider.bounds.extents.y * transform.up;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _distanceFromDeskEdge = 0.1f;
    [SerializeField] MeshFilter _meshFilter;
    [SerializeField] BoxCollider _boxCollider;
    [SerializeField] NonPlayableDominoFallingDector _nonPlayableDominoFallingDetectorPrefab;
    [Tooltip("The y offset applied to the non playable domino falling detector.")]
    [SerializeField] float _detectorHeightOffset = 0.09f;

    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    private NonPlayableDominoFallingDector _nonPlayableDominoFallingDetector;

    void Awake()
    {
        DeskSpawned?.Invoke(this);
        StartCoroutine(SetupDeskAfterEndOfFrame());
        HideDeskAndDisableDominoFallingDetector();
    }

    public void ShowDesk()
    {
        // TODO maybe an animation would be better
        _renderer.enabled = true;
    }

    public void HideDeskAndDisableDominoFallingDetector()
    {
        _renderer.enabled = false;
        DisableDominoFallingDetector();
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
        SetupDominoFallingDetector();
        SetupDone?.Invoke();
    }

    private void SetupDominoFallingDetector()
    {
        if (_nonPlayableDominoFallingDetector == null)
        {
            _nonPlayableDominoFallingDetector = Instantiate(_nonPlayableDominoFallingDetectorPrefab, transform);
            var detectorBoxCollider = _nonPlayableDominoFallingDetector.GetComponent<BoxCollider>();
            detectorBoxCollider.center = _boxCollider.center + Vector3.up * _detectorHeightOffset;
            detectorBoxCollider.size = _boxCollider.size;
        }
        _nonPlayableDominoFallingDetector.gameObject.SetActive(true);
    }

    private void DisableDominoFallingDetector()
    {
        if (_nonPlayableDominoFallingDetector == null)
        {
            return;
        }

        _nonPlayableDominoFallingDetector.gameObject.SetActive(false);
    }
}
