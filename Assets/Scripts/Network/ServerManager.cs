using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Multiplayer.Playmode;
using System.Linq;

public class ServerManager : NetworkBehaviour
{
    public static ServerManager Instance;

    [SerializeField] private NetworkObject pawnPrefab;
    private Dictionary<string, NetworkObject> connectedObjects = new();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Instance = this;
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.SceneManager.OnLoadComplete += InitPlayer;
        }
    }

    private void InitPlayer(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        Debug.Log(clientId);
        if (sceneName == "In-Game")
        {
            RpcParams rpcParams = new RpcParams() { Send = RpcTarget.Single(clientId, RpcTargetUse.Temp) };
            RequestPawnClientRpc(rpcParams);
        }
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void RequestPawnClientRpc(RpcParams rpcParams)
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        if (CurrentPlayer.ReadOnlyTags().Contains("Player 1"))
        {
            RequestPawnForClientRpc("Player 1", clientId);
        }
        else if (CurrentPlayer.ReadOnlyTags().Contains("Player 2"))
        {
            RequestPawnForClientRpc("Player 2", clientId);
        }
        else if (CurrentPlayer.ReadOnlyTags().Contains("Player 3"))
        {
            RequestPawnForClientRpc("Player 3", clientId);
        }
    }

    [Rpc(SendTo.Server)]
    public void RequestPawnForClientRpc(string playerName, ulong clientId)
    {
        Debug.Log($"Requsting pawn for cliend ID: {clientId} With Player Name of: {playerName}");
        SetPawnForPlayer(playerName, clientId);
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
    }

    public void SetPawnForPlayer(string playerName, ulong clientId)
    {
        if (!IsClientRegistered(playerName))
        {
            NetworkObject pawn = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(pawnPrefab, clientId);
            RegisterPlayer(playerName, pawn);
        }
        else
        {
            var pawnNObj = GetClientPawn(playerName);
            pawnNObj.ChangeOwnership(clientId);
        }
    }

    public bool IsClientRegistered(string username)
    {
        if (connectedObjects.ContainsKey(username))
            return true;
        return false;
    }

    public void RegisterPlayer(string username, NetworkObject playerPrefab)
    {
        connectedObjects[username] = playerPrefab;
    }

    public NetworkObject GetClientPawn(string username)
    {
        return connectedObjects[username];
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest req, NetworkManager.ConnectionApprovalResponse res)
    {
        res.CreatePlayerObject = true;
        res.Pending = false;
        res.Approved = true;
        res.Reason = "";
    }
}