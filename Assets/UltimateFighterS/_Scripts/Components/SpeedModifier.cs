using UnityEngine;

public class SpeedModifier : OwnedComponent
{
    public void SpeedModified(GameObject target)
    {
        Rigidbody ownerRigid = Owner.GetComponent<Rigidbody>();
        Rigidbody targetRigid = target.GetComponent<Rigidbody>();

        if (ownerRigid && targetRigid)
        {
            Vector3 speed10Porcent = ownerRigid.velocity * 0.1f;

            targetRigid.velocity += speed10Porcent;
        }
    }
}