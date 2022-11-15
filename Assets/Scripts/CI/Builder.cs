using UnityEditor;
using UnityEngine.SceneManagement;

public static class Builder
{
    public static void BuildAndroid()
    {
        var options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/MainScene.unity" },
            target = BuildTarget.Android,
            locationPathName = "./Build/",
        };

        BuildPipeline.BuildPlayer(options);
    }
}
