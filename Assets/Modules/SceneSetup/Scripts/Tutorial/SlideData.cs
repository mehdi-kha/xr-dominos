using UnityEngine;

[CreateAssetMenu(fileName = "SlideData", menuName = "ScriptableObjects/SlideData", order = 1)]
public class SlideData : ScriptableObject
{
    public string Title;
    public Texture Image;
    public string Text;
}
