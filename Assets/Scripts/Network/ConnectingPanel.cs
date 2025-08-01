using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectingPanel : MonoBehaviour
{

    private void OnGUI()
    {
        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Host"))
            {
                NetworkManager.Singleton.StartHost();
                NetworkManager.Singleton.SceneManager.LoadScene("In-Game",UnityEngine.SceneManagement.LoadSceneMode.Single);
                SceneManager.LoadScene("Online Scene",LoadSceneMode.Additive);
            }
            if (GUI.Button(new Rect(10, 50, 100, 30), "Client"))
            {
                NetworkManager.Singleton.StartClient();
                SceneManager.LoadScene("Online Scene", LoadSceneMode.Additive);
            }
            if (GUI.Button(new Rect(10, 90, 100, 30), "Server"))
            {
                NetworkManager.Singleton.StartServer();
                NetworkManager.Singleton.SceneManager.LoadScene("In-Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }
}