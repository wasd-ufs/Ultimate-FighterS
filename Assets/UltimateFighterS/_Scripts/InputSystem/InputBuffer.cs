using System;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class InputBuffer : MonoBehaviour
{
    [SerializeField] private InputSystem input;
    private Timer _timer;
    public InputSystemMemento Current { get; private set; }

    private void Awake()
    {
        _timer = gameObject.GetComponent<Timer>();
        _timer.onTimerFinish.AddListener(Consume);
    }

    private void Update()
    {
        if (input.IsAttackJustPressed())
            Register();

        if (input.IsSpecialJustPressed())
            Register();
    }

    public void Register()
    {
        _timer.Init();
        Current = input.GetMemento();
    }

    public void Consume()
    {
        _timer.Finish();
        Current = null;
    }

    public void ConsumeIf(Func<InputSystemMemento, bool> condition, Action<InputSystemMemento> beforeConsumption)
    {
        if (Current == null || !condition(Current))
            return;

        beforeConsumption(Current);
        Consume();
    }
}