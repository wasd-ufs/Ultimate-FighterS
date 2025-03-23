using UnityEngine;

public class DeathEffect : Effect
{
    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<DeathComponent>()?.Kill();
    }
}