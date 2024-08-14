using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerDiedNotify : Notifier
{   
    public override void Notify()
    {
        GlobalEvents.onPlayerDied.Invoke(this.gameObject);
    }
}
