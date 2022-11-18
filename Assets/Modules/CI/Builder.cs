#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class Builder
{
    public static void BuildAndroid()
    {
        var options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/MainScene.unity" },
            target = BuildTarget.Android,
            locationPathName = $"{Application.dataPath}/../Build/build.apk",
        };

        BuildPipeline.BuildPlayer(options);
    }
}
#endif
