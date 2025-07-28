using UnityEngine;
using VContainer;

public class TestForHide : MonoBehaviour, IHideable
{
    private MeshRenderer meshRenderer;

    [Inject] private readonly SceneHiderManager sceneHiderManager;
    [Inject] private readonly TestingService testingService;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void RegisterSelf()
    {
        sceneHiderManager.RegisterHideable(this);
    }

    public void Show()
    {
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }    
}