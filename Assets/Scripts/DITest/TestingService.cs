using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class TestingService
{
    public void TestMethod()
    {
        Debug.Log("Test method");
    }
}

public class TestingEntryPoint : IStartable, ITickable
{
    public TestingService service;
    public PlayerInput playerInput;
    public IObjectResolver resolver;

    public TestingEntryPoint(TestingService service,PlayerInput playerInput, IObjectResolver resolver)
    {
        this.service = service;
        this.playerInput = playerInput;
        this.resolver = resolver;
    }

    public void Start()
    {
        service.TestMethod();
        //var moveAction = playerInput.actions["Move"];

        var testingInjection = resolver.Resolve<TestingInjection>();


    }



    public void Tick()
    {
        //Debug.Log(playerInput.actions["Move"].ReadValue<Vector2>());
    }
}

public class TestingInjection
{
    public readonly PlayerInput playerInput;

    //public TestingInjection()
    //{
    //    playerInput.actions["Move"].performed += ctx =>
    //    {
    //        Debug.Log(ctx.ReadValue<Vector2>());
    //    };
    //}

    [Inject]
    public TestingInjection(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
        playerInput.actions["Move"].performed += ctx =>
        {
            Debug.Log(ctx.ReadValue<Vector2>());
        };
    }
}
