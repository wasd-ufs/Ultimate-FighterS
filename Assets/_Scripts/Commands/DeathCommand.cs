using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DeathCommand : Command
{
    public override void Run(GameObject gameObject)
    {
        gameObject.GetComponent<DeathComponent>()?.Kill();
    }
}
