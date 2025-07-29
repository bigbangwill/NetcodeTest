using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class HideEntryPoint : LifetimeScope
{
    [SerializeField] private Transform hideableTransform;

    protected override void Configure(IContainerBuilder builder)
    {
        var hideableArray = hideableTransform.GetComponentsInChildren<IHideable>();
        foreach (var hide in hideableArray)
        {
            builder.Register(_ => hide, Lifetime.Scoped);
        }

        builder.Register<Func<GameObject, IHideable>>(c => go =>
        {
            var hide = go.AddComponent<TestForHide>();
            c.InjectGameObject(go);
            return hide;

        }, Lifetime.Scoped);

        builder.Register<SceneHiderManager>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SceneHiderManager>();
    }

}