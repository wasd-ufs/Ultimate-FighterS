using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private InputSystem input;
    private InputSystemMemento current;

    void Start()
    {
        timer.onTimerFinish.AddListener(Consume);
    }

    void Update()
    {
        if (input.IsAttackJustPressed())
            Register();
        
        if (input.IsSpecialJustPressed())
            Register();
    }

    void Register()
    {
        timer.Init();
        current = input.GetMemento();
    }

    void Consume()
    {
        timer.Finish();
        current = null;
    }

    void ConsumeIf(bool condition, Action<InputSystemMemento> beforeConsumption)
    {
        if (!condition || current == null)
            return;
        
        beforeConsumption(current);
        Consume();
    }
}
