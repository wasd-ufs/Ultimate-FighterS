using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("Time")] public float waitTime = 1f;

    [HideInInspector] public float elapsedTime;

    [Header("Behaviour")] public bool startOnAwake;

    public bool restartOnFinish;

    [Header("Events")] public UnityEvent onTimerInit = new();

    public UnityEvent onTimerFinish = new();

    private void Awake()
    {
        elapsedTime = -1f;
        if (startOnAwake)
            Init();
    }

    public void Update()
    {
        if (elapsedTime <= -1f)
            return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= waitTime)
            Finish();
    }

    public void Init()
    {
        elapsedTime = 0.01f;
        onTimerInit.Invoke();
    }

    public void Finish()
    {
        if (elapsedTime >= 0f)
        {
            elapsedTime = -1f;
            onTimerFinish.Invoke();
        }

        if (restartOnFinish)
            Init();
    }

    public bool IsFinished()
    {
        return elapsedTime < 0f;
    }
}