using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
using System;
using VContainer;

public class ServerManager : NetworkBehaviour
{
    public static ServerManager Instance;

    [SerializeField] private NetworkObject pawnPrefab;
    private Dictionary<string, NetworkObject> connectedObjects = new();

    [Inject] private readonly Func<GameObject, GameObject> resolveFactory;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Instance = this;
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnClientStarted += Singleton_OnClientStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        }
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        Debug.Log("Client connected: " + obj);
    }

    private void Singleton_OnClientStarted()
    {
        Debug.Log("Started");
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
            GameObject pawnGO = Instantiate(pawnPrefab.gameObject);
            pawnGO.transform.position = new(0, 100, 0);
            NetworkObject pawnNO = pawnGO.GetComponent<NetworkObject>();
            pawnNO.GetComponent<NetworkObject>().Spawn();
            pawnNO.ChangeOwnership(clientId);
            pawnGO.GetComponent<SyncedBall>().AssignPlayerInputRPC();
            RegisterPlayer(playerName, pawnNO);
        }
        else
        {
            var pawnNObj = GetClientPawn(playerName);
            pawnNObj.ChangeOwnership(clientId);
            pawnNObj.ChangeOwnership(clientId);
            pawnNObj.GetComponent<SyncedBall>().AssignPlayerInputRPC();
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