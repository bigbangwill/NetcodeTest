using Unity.Netcode;
using UnityEngine;

public class SyncedBall : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new();

    private void Update()
    {
        if (IsOwner)
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (input != Vector3.zero)
            {
                Vector3 newPos = transform.position + input * Time.deltaTime * 5f;
                transform.position = newPos;
                SubmtPositionServerRpc(newPos);
            }
        }
        else
        {
            transform.position = Position.Value;
        }
    }

    [Rpc(SendTo.Server)]
    private void SubmtPositionServerRpc(Vector3 newPos, RpcParams rpcParams = default)
    {
        Position.Value = newPos;
    }
}