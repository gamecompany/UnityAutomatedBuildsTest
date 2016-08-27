using UnityEngine;
using UnityEditor;

#warning This is just a test warning.

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
        int unassigned;

        return "Builds";
    }

    public static string outputFilename()
    {
        return Application.productName;
    }

    public static string outputPathAndFilename(string extension)
    {
        return string.Format("{0}\\{1}.{2}", outputPathName(), outputFilename(), extension);
    }

    public static void DoBuild(string buildName, string extension, BuildTarget target)
    {
        Debug.LogFormat("Started '{0}' at {1}", buildName, System.DateTime.Now);

        System.IO.Directory.CreateDirectory(outputPathName());

        string message = BuildPipeline.BuildPlayer(
            scenesToIncludeInReleaseBuild(),
            outputPathAndFilename(extension),
            target,
            BuildOptions.ShowBuiltPlayer);

        if (string.IsNullOrEmpty(message))
        {
            Debug.LogFormat("{0} build completed successfully at {1}", buildName, System.DateTime.Now);
        }
        else
        {
            // I chose to not log the 'message' because it is already in the log
            Debug.LogErrorFormat("{0} build finished with errors, at {1}", buildName, System.DateTime.Now); 
        }
    }

    [MenuItem("Builds/Mac")]
    public static void BuildForMac() 
    {
        DoBuild("Mac OS X Build", "app", BuildTarget.StandaloneOSXUniversal);
    }

    [MenuItem("Builds/Android")]
    public static void BuildForAndroid() 
    {
        DoBuild("Android Build", "apk", BuildTarget.Android);
    }

}
