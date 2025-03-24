using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DeathEffect : BaseEffect
{
    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<DeathComponent>()?.Kill();
    }
}
