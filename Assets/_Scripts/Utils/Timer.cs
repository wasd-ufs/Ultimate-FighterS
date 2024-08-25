using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("Time")]
    public float waitTime = 1f;
    public float elapsedTime = 0f;
    
    [Header("Behaviour")]
    public bool startOnAwake = false;
    public bool restartOnFinish = false;
    
    [Header("Events")]
    public UnityEvent onTimerInit = new();
    public UnityEvent onTimerFinish = new();

    private void Start()
    {
        if (startOnAwake)
            Init();
    }

    public void Init()
    {
        elapsedTime = 0f;
        onTimerInit.Invoke();
    }

    private void Update()
    {
        if (elapsedTime < 0f)
            return;
        
        elapsedTime = Mathf.Min(waitTime, elapsedTime + Time.deltaTime);
        if (elapsedTime >= waitTime)
            Finish();
    }

    public void Finish()
    {
        elapsedTime = -1f;
        onTimerFinish.Invoke();
        
        if (restartOnFinish)
            Init();
    }
}
