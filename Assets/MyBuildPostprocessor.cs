using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

#if UNITY_IOS
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#endif

public class MyBuildPostprocessor {
  [PostProcessBuildAttribute(1)]
  public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
#if UNITY_IOS
    Debug.Log("OnPostprocessBuild: Path [{" + path + "}]");
    if (buildTarget == BuildTarget.iOS) {
      string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

      PBXProject proj = new PBXProject();
      proj.ReadFromString(File.ReadAllText(projPath));
      var mainTarget = proj.GetUnityMainTargetGuid();

      Debug.Log("OnPostprocessBuild: Adding strip script to target: [" + mainTarget + "]");
      proj.AddShellScriptBuildPhase(mainTarget, "Strip unused architectures", "/bin/sh", "Assets/Plugins/iOS/TestFairy.framework/strip-architectures.sh");

      File.WriteAllText(projPath, proj.WriteToString());
    }
#endif
  }
}
