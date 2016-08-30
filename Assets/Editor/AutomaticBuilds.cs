using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class AutomaticBuilds 
{
	const string k_TargetPlatformParam = "-targetPlatform=";
	const string k_ResultFilePathParam = "-resultFilePath=";
	static int errorCodeBadPlatform = 3;
	
    public static string[] scenesToIncludeInReleaseBuild()
    {
        return new string[] {
            "Assets/Scenes/Scene1.unity",
            "Assets/Scenes/Scene2.unity"
        };
    }

    public static string outputPathName()
    {
        return "Builds";
    }

    public static string outputFilename()
    {
        return Application.productName;
    }

	// Copy and paste reuse from PlatformRunnerConfiguration.GetTempPath() from the UnityTestTools
	public static string outputExtension(BuildTarget buildTarget)
	{
		switch (buildTarget)
		{
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneWindows64:
			return "exe";
		case BuildTarget.StandaloneOSXIntel:
			return "app";
		case BuildTarget.Android:
			return "apk";
		default:
			if (buildTarget.ToString() == "BlackBerry" || buildTarget.ToString() == "BB10")
				return "bar";
			return "";
		}
	}

	public static string outputPathAndFilename(BuildTarget targetPlatform)
    {
		return string.Format("{0}\\{1}.{2}", outputPathName(), outputFilename(), outputExtension(targetPlatform));
    }

	// This is meant to be called from the command line to automatically build the project:
	// ex. Unity -quit -batchmode -projectPath <PROJECT_PATH> -targetPlatform=<PLATFORM> -executeMethod AutomaticBuilds.Build
	// The targetPlatform comes from the BuildTarget enum, if you do not provide it, it won't work
	public static void Build()
	{
		if (GetTargetPlatform().HasValue)
		{
			BuildTarget buildTarget = GetTargetPlatform().Value;
			DoBuild(buildTarget);
		}
		else
		{
			Debug.LogError("No target platform specified");
			EditorApplication.Exit(errorCodeBadPlatform);
		} 
	}
		
    public static void DoBuild(BuildTarget targetPlatform)
    {
		string buildName = targetPlatform.ToString();

        Debug.LogFormat("Started '{0}' at {1}", buildName, System.DateTime.Now);

        System.IO.Directory.CreateDirectory(outputPathName());

        string message = BuildPipeline.BuildPlayer(
            scenesToIncludeInReleaseBuild(),
			outputPathAndFilename(targetPlatform),
            targetPlatform,
            BuildOptions.ShowBuiltPlayer | BuildOptions.AutoRunPlayer);

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

	// Copy and paste reuse from UnityTest.Batch.GetTargetPlatform()
	private static BuildTarget ? GetTargetPlatform()
	{
		string platformString = null;
		BuildTarget buildTarget;
		foreach (var arg in Environment.GetCommandLineArgs())
		{
			if (arg.ToLower().StartsWith(k_TargetPlatformParam.ToLower()))
			{
				platformString = arg.Substring(k_ResultFilePathParam.Length);
				break;
			}
		}
		try
		{
			if (platformString == null) return null;
			buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), platformString);
		}
		catch
		{
			return null;
		}
		return buildTarget;
	}

    [MenuItem("Builds/Mac")]
    public static void BuildForMac() 
    {
        DoBuild(BuildTarget.StandaloneOSXUniversal);
    }

    [MenuItem("Builds/Android")]
    public static void BuildForAndroid() 
    {
        DoBuild(BuildTarget.Android);
    }
}
