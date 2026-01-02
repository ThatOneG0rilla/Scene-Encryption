#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
public static class SceneEncryptionDebug
{
    [MenuItem("Tools/Debug Encrypt Scene")]
    public static void EncryptNow()
    {
        if (!EditorUtility.DisplayDialog(
                "Are you sure?",
                "This will encrypt all your scenes. Only use this for debug purposes.\n\nDo NOT save the scene after this, as it can lead to damage to your scene.\nTo decrypt your scene, reload your scene, or restart your editor.\nWorst case scenario, you do something wrong, your scene is backed up in the backup folder.",
                "Encrypt (You are aware of what can happen to your scene)", "Cancel"))
            return;

        for (int i = 0; i < SceneManager.sceneCount; i++)
            new SceneEncryption().OnProcessScene(SceneManager.GetSceneAt(i));

        EditorUtility.DisplayDialog(
            "Encryption Complete",
            "Do not save your scene until you restart your editor, or reload your scene. If you do, I am not at fault.",
            "OK"
        );
    }
}
#endif