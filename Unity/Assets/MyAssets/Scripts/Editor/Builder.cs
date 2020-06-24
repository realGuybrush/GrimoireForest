using UnityEditor;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static void Build()
    {
        BuildPipeline.BuildPlayer(
            EditorBuildSettings.scenes,
            "../Build/Deploy/StandaloneWindows64/GrimoireForest.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None);
    }
}
