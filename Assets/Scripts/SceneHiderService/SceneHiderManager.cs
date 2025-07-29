using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public class SceneHiderManager: IStartable
{
    private readonly List<IHideable> hideables = new();
    private readonly IEnumerable<IHideable> initialHideables;
    private readonly Func<GameObject, IHideable> InstantiateHideable;

    public SceneHiderManager(IEnumerable<IHideable> hideables, Func<GameObject,IHideable> getHideable)
    {
        initialHideables = hideables;
        InstantiateHideable = getHideable;
    }

    public void Start()
    {
        foreach (var hide in initialHideables)
        {
            hide.RegisterSelf();
            hideables.Add(hide);
        }

        GameObject go = new GameObject();
        go.AddComponent<MeshRenderer>();
        go.AddComponent<MeshFilter>();
        var hideable = InstantiateHideable(go);
        hideable.RegisterSelf();
        hideables.Add(hideable);
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