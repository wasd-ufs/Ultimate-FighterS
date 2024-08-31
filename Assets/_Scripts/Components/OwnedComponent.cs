using System;
using UnityEngine;

public abstract class OwnedComponent : MonoBehaviour
{
    private IdComponent id;
    public int Id => id?.id ?? -1;
    public GameObject Owner => id?.gameObject ?? transform.root.gameObject;

    private void Start()
    {
        id = IdComponent.GetFirstIdInHierarchy(gameObject);
    }

    public bool IsSameAs(OwnedComponent component)
     => Id == component.Id;
}