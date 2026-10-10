using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public const string Loading = "Loading";
    public const string Home = "Home";
    public const string GamePlay = "GamePlay";

    public static void Load(string sceneName)
    {
        Time.timeScale = 1f;

        if (GameController.Instance != null)
            GameController.Instance.LoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }
}
