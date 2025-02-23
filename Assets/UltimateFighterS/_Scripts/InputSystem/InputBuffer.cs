using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class InputBuffer : MonoBehaviour
{
    [SerializeField] private InputSystem input;
    public InputSystemMemento Current { get; private set; }
    private Timer timer;

    void Awake()
    {
        timer = gameObject.GetComponent<Timer>();
        timer.onTimerFinish.AddListener(Consume);
    }

    void Update()
    {
        if (input.IsAttackJustPressed())
            Register();
        
        if (input.IsSpecialJustPressed())
            Register();
    }

    public void Register()
    {
        timer.Init();
        Current = input.GetMemento();
    }

    public void Consume()
    {
        timer.Finish();
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
