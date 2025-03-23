using UnityEngine;

public class IdComponent : MonoBehaviour
{
    public int id;

    public static IdComponent GetFirstIdInHierarchy(GameObject baseObj)
    {
        if (baseObj == null)
            return null;

        while (true)
        {
            IdComponent id = baseObj.GetComponent<IdComponent>();
            if (id != null)
                return id;

            if (baseObj.transform == baseObj.transform.root)
                return null;

            baseObj = baseObj.transform.parent.gameObject;
        }
    }
}