using UnityEngine;

public class FallingDominoDetector : MonoBehaviour
{
    [SerializeField] private string _dominoTag;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != _dominoTag)
        {
            return;
        }
        Debug.Log("A domino has fallen!");
    }
}
