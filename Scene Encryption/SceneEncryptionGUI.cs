#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
public sealed class SceneEncryptionGUI : EditorWindow
{
    public static string BackupFolder => Path.GetFullPath(Path.Combine(Application.dataPath, "..", "SceneBackups"));
    [MenuItem("Tools/Scene Encryption")]
    static void Open()
    {
        GetWindow<SceneEncryptionGUI>("Scene Encryption");
        SceneEncryption.EnableObfuscation = EditorPrefs.GetBool("SceneEncryption.EnableObfuscation", true);
        SceneEncryption.SkipAnimators = EditorPrefs.GetBool("SceneEncryption.SkipAnimators", true);
        SceneEncryption.IgnoreLayer = EditorPrefs.GetInt("SceneEncryption.IgnoreLayer", 0);
        SceneEncryption.Characters = EditorPrefs.GetString("SceneEncryption.Characters", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        SceneEncryption.NameLength = EditorPrefs.GetInt("SceneEncryption.NameLength", 12);
        SceneEncryption.EnableBackups = EditorPrefs.GetBool("SceneEncryption.EnableBackups", true);
    }
    void OnGUI()
    {
        GUILayout.Space(6);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Scene Obfuscation", EditorStyles.boldLabel);

        SceneEncryption.EnableObfuscation = EditorGUILayout.Toggle("Enable Obfuscation", SceneEncryption.EnableObfuscation);
        SceneEncryption.SkipAnimators = EditorGUILayout.Toggle("Skip Animators", SceneEncryption.SkipAnimators);
        SceneEncryption.IgnoreLayer = EditorGUILayout.LayerField("Ignore Layer", SceneEncryption.IgnoreLayer);
        SceneEncryption.NameLength = EditorGUILayout.IntSlider("Name Length", SceneEncryption.NameLength, 4, 32);
        SceneEncryption.Characters = EditorGUILayout.TextField("Random Characters", SceneEncryption.Characters);

        EditorPrefs.SetBool("SceneEncryption.EnableObfuscation", SceneEncryption.EnableObfuscation);
        EditorPrefs.SetBool("SceneEncryption.SkipAnimators", SceneEncryption.SkipAnimators);
        EditorPrefs.SetInt("SceneEncryption.IgnoreLayer", SceneEncryption.IgnoreLayer);
        EditorPrefs.SetInt("SceneEncryption.NameLength", SceneEncryption.NameLength);
        EditorPrefs.SetString("SceneEncryption.Characters", SceneEncryption.Characters);

        EditorGUILayout.HelpBox(
                   "Scene obfuscation uses a different method ensuring no risk to your scene\n" +
                   "Scenes changes are never saved on disk.",
                   MessageType.Info
            );
        EditorGUILayout.EndVertical();

        GUILayout.Space(8);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Scene Opt-Out", EditorStyles.boldLabel);

        EditorBuildSettingsScene[] BuildScenes = EditorBuildSettings.scenes;
        if (BuildScenes.Length == 0)
            EditorGUILayout.HelpBox("No scenes found in Build Settings.", MessageType.Warning);

        for (int SceneIndex = 0; SceneIndex < BuildScenes.Length; SceneIndex++)
        {
            string ScenePath = BuildScenes[SceneIndex].path;
            bool Excluded = EditorPrefs.GetBool("SceneEncryption.Exclude." + ScenePath, false);
            bool NewExcluded = EditorGUILayout.ToggleLeft(ScenePath, Excluded);
            if (NewExcluded != Excluded)
                EditorPrefs.SetBool("SceneEncryption.Exclude." + ScenePath, NewExcluded);
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(8);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Backups", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "Scene backups as a safety precaution. They should not be needed, but just incase, they are an option.",
            MessageType.None
        );

        SceneEncryption.EnableBackups = EditorGUILayout.Toggle("Enable Backups", SceneEncryption.EnableBackups);
        EditorPrefs.SetBool("SceneEncryption.EnableBackups", SceneEncryption.EnableBackups);
        if (GUILayout.Button("Open Backups Folder", GUILayout.Height(28)))
        {
            if (!Directory.Exists(BackupFolder))
                Directory.CreateDirectory(BackupFolder);

            EditorUtility.RevealInFinder(BackupFolder);
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Join the Discord", GUILayout.Width(160), GUILayout.Height(28)))
            Application.OpenURL("https://discord.gg/96Tffxj92Z");
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }
}
#endif