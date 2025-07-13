using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectedPanel : NetworkBehaviour
{
    private void OnGUI()
    {
        if (IsSpawned)
        {
            if (GUI.Button(new Rect(10, 160, 120, 30), "Scene 1"))
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Scene 1", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }

            if (GUI.Button(new Rect(10, 190, 120, 30), "Scene 2"))
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Scene 2", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }

            if (GUI.Button(new Rect(10, 220, 120, 30), "Sample Scene"))
            {
                NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }

            if (GUI.Button(new Rect(10, 250, 120, 30), "Disconnect"))
            {
                BackToMainMenuRpc();
                StartCoroutine(WaitForClientsToDisconnect());
            }
        }
    }

    private IEnumerator WaitForClientsToDisconnect()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.ConnectedClients.Count == 1);
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("SampleScene");
    }

    [Rpc(SendTo.NotMe)]
    private void BackToMainMenuRpc()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("SampleScene");
    }

}