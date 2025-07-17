using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectedPanel : NetworkBehaviour
{
    public static ConnectedPanel Instance;

    private void OnGUI()
    {
        if (IsSpawned)
        {
            if (GUI.Button(new Rect(10, 160, 120, 30), "Scene 1"))
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Scene 1", LoadSceneMode.Single);
            }

            if (GUI.Button(new Rect(10, 190, 120, 30), "Scene 2"))
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Scene 2", LoadSceneMode.Single);
            }

            if (GUI.Button(new Rect(10, 220, 120, 30), "Sample Scene"))
            {
                NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            }

            if (GUI.Button(new Rect(10, 250, 120, 30), "Disconnect"))
            {
                BackToMainMenu();
                //StartCoroutine(WaitForClientsToDisconnect());
            }
        }
    }

    private void BackToMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene("StartingScene");
    }
}