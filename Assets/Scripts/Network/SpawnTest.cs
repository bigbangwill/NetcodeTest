using Unity.Netcode;
using UnityEngine;

public class SpawnTest : NetworkBehaviour
{
    [SerializeField] private GameObject prefab;

    private void OnGUI()
    {
        if (IsSpawned)
        {
            if (GUI.Button(new Rect(10, 130, 120, 30), "Spawn Cube"))
            {
                RequestCubeRpc();
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestCubeRpc(RpcParams rpcParams = default)
    {
        ulong requester = rpcParams.Receive.SenderClientId;
        GameObject go = Instantiate(prefab, new Vector3(Random.Range(-3, 3), 1, 0), Quaternion.identity);
        var netObj = go.GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership(requester);
    }
}