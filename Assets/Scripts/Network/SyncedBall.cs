using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class SyncedBall : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new();

    private PlayerInput playerInput;

    [Rpc(SendTo.Owner)]
    public void AssignPlayerInputRPC()
    {
        playerInput = LifetimeScope.Find<ServerLifeTimeScope>().Container.Resolve<PlayerInput>();
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (playerInput == null)
            {
                Debug.LogWarning("Player input is null");
                return;
            }

            Vector2 playerInputValue = playerInput.actions["Move"].ReadValue<Vector2>();

            Vector3 input = new Vector3(playerInputValue.x, 0, playerInputValue.y);
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