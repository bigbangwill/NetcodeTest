using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class RootLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<TestingService>(Lifetime.Singleton);
        builder.RegisterEntryPoint<TestingEntryPoint>();
        builder.RegisterComponent(GetComponent<PlayerInput>());
        builder.Register<TestingInjection>(Lifetime.Scoped);
    }
}