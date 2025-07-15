using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class ServerManager
{
    private Dictionary<string, NetworkObject> connectedObjects = new();

    public bool IsClientRegistered(string username)
    {
        if (connectedObjects.ContainsKey(username))
            return true;
        return false;
    }

    public void ClientFirstTimeConnected(string username, NetworkObject playerPrefab)
    {
        connectedObjects[username] = playerPrefab;
    }

    public NetworkObject GetClientPlayerPrefab(string username)
    {
        return connectedObjects[username];
    }    
}