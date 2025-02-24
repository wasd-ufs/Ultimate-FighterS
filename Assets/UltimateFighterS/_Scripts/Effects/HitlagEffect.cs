using UnityEngine;

public class HitlagEffect : Effect
{
    [SerializeField] private float lagTime;
    [SerializeField] private float hitstopTime = 2000f;
    
    public override void Apply(GameObject target) => target.GetComponent<HitlagComponent>()?.Apply(lagTime, hitstopTime);
}

