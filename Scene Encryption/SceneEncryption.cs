#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine.SceneManagement;
using System;
using System.IO;
public sealed class SceneEncryption : IProcessScene
{
    public static bool EnableObfuscation = true;
    public static bool SkipAnimators = true;
    public static bool BackupDone;
    public static bool EnableBackups = true;
    public static int IgnoreLayer;
    public static int NameLength = 12;
    public static string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    public static string BackupRoot => Path.GetFullPath(Path.Combine(Application.dataPath, "..", "SceneBackups"));
    public static string ExcludePrefPrefix = "SceneEncryption.Exclude.";
    public int callbackOrder => 0;
    public void OnProcessScene(Scene Scene)
    {
        if (EnableBackups && !BackupDone)
        {
            Directory.CreateDirectory(BackupRoot);
            string Target = Path.Combine(BackupRoot, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fffffff"));
            Directory.CreateDirectory(Target);

            foreach (EditorBuildSettingsScene AllScenes in EditorBuildSettings.scenes)
                if (AllScenes.enabled && File.Exists(AllScenes.path))
                    File.Copy(AllScenes.path, Path.Combine(Target, Path.GetFileName(AllScenes.path)), true);

            BackupDone = true;
        }

        if (!EnableObfuscation || EditorPrefs.GetBool(ExcludePrefPrefix + Scene.path, false))
            return;

        foreach (GameObject Root in Scene.GetRootGameObjects())
            foreach (Transform Item in Root.GetComponentsInChildren<Transform>(true))
            {
                if (Item.gameObject.layer == IgnoreLayer)
                    continue;

                if (SkipAnimators && Item.GetComponent<Animator>())
                    continue;

                Item.name = CreateObjectName();
            }
    }
    static string CreateObjectName()
    {
        int NameLength = UnityEngine.Random.Range(8, 14);
        char[] Name = new char[NameLength];
        for (int Index = 0; Index < NameLength; Index++)
            Name[Index] = Characters[UnityEngine.Random.Range(0, Characters.Length)];
        return new string(Name);
    }
}
#endif