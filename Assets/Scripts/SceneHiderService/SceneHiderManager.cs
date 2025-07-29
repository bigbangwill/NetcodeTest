using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer.Unity;

public class SceneHiderManager
{
    private readonly List<IHideable> hideables = new();

    public SceneHiderManager(IEnumerable<IHideable> initialHideables)
    {
        foreach (var hide in initialHideables)
        {
            hide.RegisterSelf();
            hideables.Add(hide);
        }
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
        Debug.Log(hideables.Count);
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