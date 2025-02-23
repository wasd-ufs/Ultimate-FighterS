using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : OwnedComponent
{
    public void SpeedModified (GameObject target) {
        Rigidbody OwnerRigid = Owner.GetComponent<Rigidbody>();
        Rigidbody targetRigid = target.GetComponent<Rigidbody>();

        if (OwnerRigid && targetRigid) {        
            Vector3 Speed10Porcent = OwnerRigid.velocity * 0.1f;

            targetRigid.velocity += Speed10Porcent;
        }
    }
}
