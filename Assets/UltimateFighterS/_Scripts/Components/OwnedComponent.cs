using UnityEngine;

public abstract class OwnedComponent : MonoBehaviour
{
    private IdComponent _id;
    public int Id => _id?.id ?? -1;
    public GameObject Owner => _id?.gameObject ?? transform.root.gameObject;

    private void Start()
    {
        _id = IdComponent.GetFirstIdInHierarchy(gameObject);
    }

    public bool IsSameAs(OwnedComponent component)
    {
        return Id == component.Id;
    }
}