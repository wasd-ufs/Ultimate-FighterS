using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DirectionalTrigger : MonoBehaviour
{
    public List<DirectionalEvent> events = new();

    public void Trigger(Vector2 direction)
    {
        if (events.Count == 0)
            return;

        if (direction.sqrMagnitude < 0.01f)
        {
            try
            {
                var first = events.First(e => e.direction.sqrMagnitude < 0.01f);
                first.directionalEvent.Invoke();
            }
            catch
            {
                return;
            }

            return;
        }
        
        var closest = events
            .Where(e => e.direction.sqrMagnitude >= 0.01f)
            .OrderByDescending(e => Vector2.Dot(e.direction, direction) / e.direction.sqrMagnitude)
            .First(); 
        
        closest.directionalEvent.Invoke();
    }
}

[Serializable]
public class DirectionalEvent
{
    public Vector2 direction;
    public UnityEvent directionalEvent;
}
