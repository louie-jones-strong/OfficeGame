#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public static class BuildManger
{
	[MenuItem("Shared Tools/Build Manger/WebGL Build")]
	static void CreateWebGLBuild()
	{
		CreateBuild(BuildTarget.WebGL, false);
	}

	[MenuItem("Shared Tools/Build Manger/Windows Build")]
	static void CreateWindowBuild()
	{
		CreateBuild(BuildTarget.StandaloneWindows64, false);
	}


	static void CreateBuild(BuildTarget buildTarget, bool isDevBuild)
	{
		Console.WriteLine ($"Starting Build Target: {buildTarget}, isDev: {isDevBuild}");

		var buildPath = GetBuildPath(buildTarget, isDevBuild, Application.version);

		Console.WriteLine("Build path:" + buildPath);

		var buildPlayerOptions =  new BuildPlayerOptions
		{
			scenes = GetEnabledScenes(),
			locationPathName = buildPath,
			target = buildTarget,
			options = GetBuildOptions(isDevBuild)
		};

		var buildInfo = BuildPipeline.BuildPlayer(buildPlayerOptions);

		if( buildInfo.summary.result == BuildResult.Succeeded)
		{
			Console.WriteLine(":: Done with build");
		}
		else
		{
			Console.WriteLine(":: Build error");
			foreach (var step in buildInfo.steps)
			{
				foreach (var message in step.messages)
				{
					Console.WriteLine (step.name + " -- " + message.content);
				}
			}
			EditorApplication.Exit(1);
		}
	}

	static string[] GetEnabledScenes()
	{
		var sceneList = new List<string>(EditorBuildSettings.scenes.Length);

		foreach(var scene in EditorBuildSettings.scenes)
		{
			if(scene.enabled)
			{
				sceneList.Add(scene.path);
			}
		}


		return sceneList.ToArray();
	}

	static BuildOptions GetBuildOptions(bool isDevBuild)
	{
		return isDevBuild ? BuildOptions.Development : BuildOptions.None;
	}

	static string GetBuildPath(BuildTarget buildTarget, bool isDevBuild, string version)
	{
		var path = $"{Application.productName}_";

		path += buildTarget.ToString();

		if (isDevBuild)
		{
			path += "_Dev_";
		}
		else
		{
			path += "_Release_";
		}

		path += version;

		return Path.Combine("Build", path, path);
	}
}
#endif