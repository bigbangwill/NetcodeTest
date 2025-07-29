using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class SyncedBall : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new();

    private PlayerInput playerInput;
    private SceneHiderManager sceneHiderManager;

    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    private bool isHidden;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    [Rpc(SendTo.Owner)]
    public void AssignPlayerInputRPC()
    {
        playerInput = LifetimeScope.Find<ServerLifeTimeScope>().Container.Resolve<PlayerInput>();
        sceneHiderManager = LifetimeScope.Find<HideEntryPoint>().Container.Resolve<SceneHiderManager>();
        playerInput.actions["Jump"].performed += SyncedBall_performed;
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

    private void SyncedBall_performed(InputAction.CallbackContext obj)
    {
        isHidden = !isHidden;
        if (isHidden)
        {
            sceneHiderManager.HideElements();
            rb.isKinematic = true;
            meshRenderer.enabled = false;
            SceneManager.UnloadSceneAsync("Online Scene");
            SceneManager.LoadScene("Local Scene", LoadSceneMode.Additive);
        }
        else
        {
            sceneHiderManager.ShowElements();
            rb.isKinematic = false;
            meshRenderer.enabled = true;
            SceneManager.UnloadSceneAsync("Local Scene");
            SceneManager.LoadScene("Online Scene", LoadSceneMode.Additive);
        }
    }

    [Rpc(SendTo.Server)]
    private void SubmtPositionServerRpc(Vector3 newPos, RpcParams rpcParams = default)
    {
        Position.Value = newPos;
    }
}