using UnityEngine;

[CreateAssetMenu(fileName = "ColorReference", menuName = "ScriptableObjects/ColorReference", order = 1)]
public class ColorReference : ScriptableObject
{
    public Color Value;

    ColorReference(Color color)
    {
        Value = color;
    }
}
