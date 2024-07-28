using UnityEngine;

public abstract class Command : MonoBehaviour
{
    public abstract void Run(GameObject gameObject);
}