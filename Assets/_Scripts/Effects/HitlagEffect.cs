using UnityEngine;

public class HitlagEffect : Effect
{
    [SerializeField] private float lagTime;
    
    public override void Apply(GameObject target) => target.GetComponent<HitlagComponent>()?.Apply(lagTime);
}

