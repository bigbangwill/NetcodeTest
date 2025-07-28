using UnityEngine;
using UnityEngine.SceneManagement;

public class SwappingScene : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Test1" || scene.name == "Test2" || scene.name == "Test3")
        {
            SceneManager.SetActiveScene(scene);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 100), "1"))
            SceneManager.LoadScene("Test1", LoadSceneMode.Additive);

        if (GUI.Button(new Rect(100, 200, 100, 100), "2"))
            SceneManager.LoadScene("Test2", LoadSceneMode.Additive);

        if (GUI.Button(new Rect(100, 300, 100, 100), "3"))
            SceneManager.LoadScene("Test3", LoadSceneMode.Additive);
    }
}