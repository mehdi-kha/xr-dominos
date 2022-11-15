using UnityEditor;
using UnityEngine.SceneManagement;

public static class Builder
{
    public static void BuildProject(string path, BuildTarget buildTarget)
    {
        var options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/MainScene.unity" },
            target = buildTarget,
            locationPathName = path,
        };

        BuildPipeline.BuildPlayer(options);
    }
}
