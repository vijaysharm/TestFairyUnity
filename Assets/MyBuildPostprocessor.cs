using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;


public class MyBuildPostprocessor {
	internal static void CopyAndReplaceDirectory(string srcPath, string dstPath)
	{
		if (Directory.Exists(dstPath))
			Directory.Delete(dstPath);
		if (File.Exists(dstPath))
			File.Delete(dstPath);

		Directory.CreateDirectory(dstPath);

		foreach (var file in Directory.GetFiles(srcPath))
			File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

		foreach (var dir in Directory.GetDirectories(srcPath))
			CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
	}

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
		Debug.Log("OnPostprocessBuild: Path [{" + path + "}]");
		if (buildTarget == BuildTarget.iOS) {
			string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

			PBXProject proj = new PBXProject();
			proj.ReadFromString(File.ReadAllText(projPath));

			string target = proj.TargetGuidByName("Unity-iPhone");
			var script = proj.ShellScriptByName(target, "Strip unused architectures");

			if (script == null) {
				Debug.Log("OnPostprocessBuild: Adding strip script");
				script = "Assets/Plugins/iOS/TestFairy.framework/strip-architectures.sh";
				proj.AppendShellScriptBuildPhase(target, "Strip unused architectures", "/bin/sh", script);
			} else {
				Debug.Log("OnPostprocessBuild: Strip script already exists");
			}

			File.WriteAllText(projPath, proj.WriteToString());
		}
	}
}
