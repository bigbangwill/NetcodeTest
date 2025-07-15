using Unity.Netcode;
using UnityEngine;

public class ConnectionManager : NetworkBehaviour
{
    private void OnGUI()
    {
        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Host"))
            {
                NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
                NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes("BigBang");
                NetworkManager.Singleton.StartHost();
            }
            if (GUI.Button(new Rect(10, 50, 100, 30), "Client"))
            {
                NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes("Player 1");
                NetworkManager.Singleton.StartClient();
            }
            if (GUI.Button(new Rect(10, 90, 100, 30), "Server"))
            {
                NetworkManager.Singleton.StartServer();
            }
        }
    }

    private void Awake()
    {
        Debug.Log("1");
        //if (!IsServer)
        //{
        //    Debug.Log("2");
        //    NetworkManager.Singleton.OnClientDisconnectCallback += clientId =>
        //    {
        //        string reason = NetworkManager.Singleton.DisconnectReason;
        //        if (!string.IsNullOrEmpty(reason))
        //        {
        //            Debug.LogWarning($"Connection denied: {reason}");
        //        }
        //    };
        //}
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest req, NetworkManager.ConnectionApprovalResponse res)
    {
        string clientUsername = System.Text.Encoding.UTF8.GetString(req.Payload);

        bool isRegistered = ConnectedPanel.Instance.serverManager.IsClientRegistered(clientUsername);

        if (isRegistered)
        {
            res.CreatePlayerObject = false;
            var playerPrefab = ConnectedPanel.Instance.serverManager.GetClientPlayerPrefab(clientUsername);
            playerPrefab.ChangeOwnership(req.ClientNetworkId);
        }
        else
        {
            res.CreatePlayerObject = true;
            res.PlayerPrefabHash = 598201774;
            res.Position = Vector3.zero;
            res.Rotation = Quaternion.identity;
        }
        res.Pending = false;
        res.Approved = true;
        res.Reason = "";
    }
}