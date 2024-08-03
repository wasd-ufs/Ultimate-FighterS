using UnityEngine;

public class HitlagCommand : Command
{
    [SerializeField] private float lagTime;

    public override void Run(GameObject target) => target.GetComponent<HitlagComponent>()?.Apply(lagTime);
}

