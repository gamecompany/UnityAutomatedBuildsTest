using UnityEngine;
using UnityEditor;

public class AutomaticBuilds 
{
    public static string[] scenesToIncludeInReleaseBuild()
    {
        return new string[] {
            "Assets/Scenes/Scene1.unity",
            "Assets/Scenes/Scene2.unity"
        };
    }

    public static string outputPathName()
    {
        return "Builds/Game";
    }

    public static void DoBuild(string buildName, BuildTarget target)
    {
        string message = BuildPipeline.BuildPlayer(
            scenesToIncludeInReleaseBuild(),
            outputPathName(),
            target,
            BuildOptions.ShowBuiltPlayer | BuildOptions.AutoRunPlayer);

        if (string.IsNullOrEmpty(message))
        {
            Debug.LogFormat("{0} build completed successfully.", buildName);
        }
        else
        {
            Debug.LogErrorFormat("{0} build had errors.:\n{1}", buildName, message); 
        }
    }

    [MenuItem("Builds/Mac")]
    public static void BuildForMac() 
    {
        DoBuild("Mac OS X Build", BuildTarget.StandaloneOSXUniversal);
    }

    [MenuItem("Builds/Android")]
    public static void BuildForAndroid() 
    {
        DoBuild("Android Build", BuildTarget.Android);
    }

}
