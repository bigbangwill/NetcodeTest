using UnityEngine;
using VContainer;

public class TestForHide : MonoBehaviour, IHideable
{
    private MeshRenderer meshRenderer;

    public void RegisterSelf()
    {
        meshRenderer = GetComponent<MeshRenderer>();
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