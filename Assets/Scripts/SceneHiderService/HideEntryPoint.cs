using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class HideEntryPoint : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<TestForHide>();
        builder.Register<SceneHiderManager>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SceneHiderManager>();
        builder.RegisterBuildCallback(c => c.Resolve<IEnumerable<TestForHide>>());

    }
}