using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class NetworkPlayerScript : NetworkBehaviour
{
    //public static NetworkPlayerScript Instance;

    [SerializeField] private NetworkObject pawnPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsClient && IsOwner)
        {
            HandleClientSideLogic();
            if (IsHost)
            {
                NetworkManager.Singleton.SceneManager.OnLoadComplete += SceneManager_OnLoadComplete;
            }
            else if (IsClient)
            {
                NetworkManager.Singleton.SceneManager.OnSynchronizeComplete += SceneManager_OnSynchronizeComplete;
            }
        }
    }

    private void SceneManager_OnLoadComplete(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
    {
        if (sceneName == "In-Game" && clientId == NetworkManager.Singleton.LocalClientId)
        {
            SceneManager_OnSynchronizeComplete(clientId);
        }
    }

    private void SceneManager_OnSynchronizeComplete(ulong clientId)
    {
        if (clientId == NetworkManager.LocalClientId)
        {
            RequestPawnForPlayerRPC(HELPER.GetPlayerName(), clientId);
        }
    }

    private void HandleClientSideLogic()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += clientId =>
        {
            string reason = NetworkManager.Singleton.DisconnectReason;
            if (!string.IsNullOrEmpty(reason))
            {
                Debug.LogWarning($"Connection denied: {reason}");
            }
        };
    }

    [Rpc(SendTo.Server)]
    private void RequestPawnForPlayerRPC(string playername, ulong clientId)
    {
        Debug.Log(playername + " " + clientId);
        ServerManager.Instance.SetPawnForPlayer(playername, clientId);
    }
}