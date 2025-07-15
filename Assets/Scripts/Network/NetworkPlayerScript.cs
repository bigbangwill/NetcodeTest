using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerScript : NetworkBehaviour
{
    //public static NetworkPlayerScript Instance;

    [SerializeField] private NetworkObject pawnPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsClient && IsOwner)
            HandleClientSideLogic();
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
}