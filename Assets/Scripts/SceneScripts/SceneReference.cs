using UnityEngine.SceneManagement;
using System;

[Serializable]
public class SceneReference
{
    public string SceneName;

    public int SceneBuildIndex
    {
        get
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                if (name == SceneName) return i;
            }
            return -1;
        }
    }
}