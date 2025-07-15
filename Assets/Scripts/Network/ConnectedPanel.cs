using System.Collections;
using Unity.Multiplayer.Playmode;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ConnectedPanel : NetworkBehaviour
{
    public static ConnectedPanel Instance;

    public ServerManager serverManager;

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
                BackToMainMenu();
                StartCoroutine(WaitForClientsToDisconnect());
            }
        }
    }


    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Instance = this;
        }

        if (IsServer)
        {
            serverManager = new();
        }

        if (IsClient)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += clientId =>
            {
                if (!IsOwner)
                {
                    Debug.LogWarning("Is Not the Owner");
                    return;
                }
                if (CurrentPlayer.ReadOnlyTags().Contains("Player 1"))
                {
                    Debug.Log("PLayer 1");
                    PlayerJoinedTheGameRpc("Player 1", clientId);
                }
                else if (CurrentPlayer.ReadOnlyTags().Contains("Player 2"))
                {
                    Debug.Log("PLayer 2");
                    PlayerJoinedTheGameRpc("Player 2", clientId);
                }
            };
        }
    }

    [Rpc(SendTo.Server)]
    private void PlayerJoinedTheGameRpc(string playerName, ulong clientId)
    {
        Debug.Log($"Name: {playerName} Client ID: {clientId}");
        if (serverManager.IsClientRegistered(playerName))
        {
            var playerPrefab = serverManager.GetClientPlayerPrefab(playerName);
            playerPrefab.ChangeOwnership(clientId);
        }
        else
        {
            var playerPrefab = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            serverManager.ClientFirstTimeConnected(playerName, playerPrefab);
        }
    }

    private IEnumerator WaitForClientsToDisconnect()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.ConnectedClients.Count == 1);
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("SampleScene");
    }

    private void BackToMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("SampleScene");
    }

}