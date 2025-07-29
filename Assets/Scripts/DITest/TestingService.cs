using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class TestingService
{
    private int i = 0;
    public void TestMethod()
    {
        i++;
        Debug.Log("Test method " + i);
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

    [Inject]
    public TestingInjection(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
        playerInput.actions["Move"].performed += ctx =>
        {
            //Debug.Log(ctx.ReadValue<Vector2>());
        };
    }
}
