using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class ServerLifeTimeScope : LifetimeScope
{
    [SerializeField] private ServerManager serverManager;
    [SerializeField] private PlayerInput playerInput;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(serverManager);
        builder.RegisterComponent(playerInput);
        builder.Register<Func<GameObject, GameObject>>(c => go => 
        {
            c.InjectGameObject(go);
            return go;
        },Lifetime.Scoped);
    }
}