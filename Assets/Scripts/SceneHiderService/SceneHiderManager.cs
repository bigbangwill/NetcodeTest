using System.Collections.Generic;
using VContainer.Unity;

public class SceneHiderManager: IStartable
{
    private readonly List<IHideable> hideables = new();
    private readonly IEnumerable<TestForHide> initialHideables;

    public SceneHiderManager(IEnumerable<TestForHide> hideables)
    {
        initialHideables = hideables;
    }

    public void Start()
    {
        foreach (var hide in initialHideables)
        {
            hideables.Add(hide);
        }
        HideElements();
    }

    public void RegisterHideable(IHideable hideable)
    {
        if (!hideables.Contains(hideable))
        {
            hideables.Add(hideable);
        }
    }

    public void UnResgierHideable(IHideable hideable)
    {
        if (hideables.Contains(hideable))
        {
            hideables.Remove(hideable);
        }
    }

    public void ShowElements()
    {
        foreach (var hideable in hideables)
        {
            hideable.Show();
        }
    }

    public void HideElements()
    {
        foreach (var hideable in hideables)
        {
            hideable.Hide();
        }
    }
}