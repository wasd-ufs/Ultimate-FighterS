using UnityEngine;
using UnityEngine.Events;

public class ApplyToOwner : OwnedComponent
{
    [SerializeField] private UnityEvent<GameObject> toApply;

    public void Apply()
    {
        toApply.Invoke(Owner);
    }
}